/*
 * MIT License
 *
 * Copyright 2016-2023 Open Text
 *
 * The only warranties for products and services of Open Text and
 * its affiliates and licensors ("Open Text") are as may be set forth
 * in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or
 * omissions contained herein. The information contained herein is subject
 * to change without notice.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using PSModule.ParallelRunner.SDK.Entity;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PSModule.ParallelRunner.SDK.Util
{
    using C = Common.Constants;
    public static class Helper
    {
        private static readonly string[] _toBeIgnoredStatuses = new string[] { "Running", "Pending" };
        private const string HTML_SCRIPT_XPATH = "/html/script";
        private const string VAR_JSON_PREFIX = "var json_";
        private const string JSON_END_SUFFIX = "}};";
        public const string FILE_NOT_FOUND = "ParallelRunner html report file not found";

        private static readonly char[] EQ = new char[] { '=' };
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { ContractResolver = new DefaultContractResolver() };

        public static List<TestRun> GetTestRuns(string reportPath)
        {
            if (HasParallelRunnerJsonReport(reportPath, out string fullPathFilename))
            {
                var runs = GetTestRunsFromJson(fullPathFilename);
                if (runs.Any())
                    return runs;
            }
            fullPathFilename = Path.Combine(reportPath, C.PARALLEL_RUN_RESULTS_HTML);
            return File.Exists(fullPathFilename) ? 
                GetTestRunsFromHtml(fullPathFilename): 
                throw new FileNotFoundException(FILE_NOT_FOUND, fullPathFilename);
        }

        /// <summary>
        /// Check if the run_results.html exists in the specified path
        /// </summary>
        /// <param name="parallelRptPath">test's ParallelReport subdir, like "D:\Work\UFTTests\SuccessWait\ParallelReport\Res18\SuccessWait_[1]"</param>
        /// <returns></returns>
        public static bool HasUftHtmlReport(string parallelRptPath)
        {
            return File.Exists(Path.Combine(parallelRptPath, $@"Report\{C.RUN_RESULTS_HTML}"));
        }

        private static bool HasParallelRunnerJsonReport(string parallelRptResPath, out string fullPathFilename)
        {
            fullPathFilename = Path.Combine(parallelRptResPath, C.PARALLEL_RUN_RESULTS_JSON);
            return File.Exists(fullPathFilename);
        }

        private static List<TestRun> GetTestRunsFromJson(string fullPathFilename)
        {
            using StreamReader sr = new(fullPathFilename);
            string json = sr.ReadToEnd();
            TestRun[] tests = json.FromJson<TestRun[]>(_jsonSerializerSettings);
            return tests?.Where(t => t != null).OrderBy(r => r.Path).ToList() ?? new();
        }
        private static List<TestRun> GetTestRunsFromHtml(string fullPathFilename)
        {
            Dictionary<int, TestRun> dict = new();
            using StreamReader sr = new(fullPathFilename);
            HtmlDocument doc = new();
            doc.Load(sr);
            var nodes = doc.DocumentNode.SelectNodes(HTML_SCRIPT_XPATH);
            if (nodes != null)
            {
                foreach (var node in nodes.Where(s => s.InnerText.TrimStart().StartsWith(VAR_JSON_PREFIX)))
                {
                    var str = node.InnerText.Split(EQ, 2)[1];
                    var len = str.IndexOf(JSON_END_SUFFIX) + 2;
                    var json = str.Substring(0, len);
                    TestRun obj = json.FromJson<TestRun>(_jsonSerializerSettings);
                    if (obj == null || obj.Status.In(_toBeIgnoredStatuses))
                        continue;
                    if (dict.ContainsKey(obj.Id))
                    {
                        dict[obj.Id] = obj;
                    }
                    else
                    {
                        dict.Add(obj.Id, obj);
                    }
                }
            }

            return dict.Values.OrderBy(r => r.RunResultsHtmlRelativePath).ToList();
        }
    }
}