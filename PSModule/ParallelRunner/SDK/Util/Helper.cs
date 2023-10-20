/*
 * Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
 * This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
 * Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
 * __________________________________________________________________
 * MIT License
 *
 * Copyright 2012-2023 Open Text
 *
 * The only warranties for products and services of Open Text and
 * its affiliates and licensors ("Open Text") are as may be set forth
 * in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or
 * omissions contained herein. The information contained herein is subject
 * to change without notice.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
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