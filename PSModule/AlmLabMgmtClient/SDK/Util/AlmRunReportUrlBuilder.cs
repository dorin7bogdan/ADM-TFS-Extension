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

using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Request;
using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    public class AlmRunReportUrlBuilder
    {
        public async Task<string> Build(IClient client, string domain, string project, string runId)
        {
            string url = "NA";
            try
            {
                AlmVersion version = await GetAlmVersion(client);
                int majorVersion = int.Parse(version.MajorVersion);
                int minorVersion = int.Parse(version.MinorVersion);
                if (majorVersion < 12 || (majorVersion == 12 && minorVersion < 2))
                    url = client.BuildWebUIEndpoint($"lab/index.jsp?processRunId={runId}");
                else if (majorVersion >= 16) // Url change due to angular js upgrade from ALM16
                    url = client.ServerUrl.AppendSuffix($"ui/?redirected&p={domain}/{project}&execution-report#!/test-set-report/{runId}");
                else
                    url = client.ServerUrl.AppendSuffix($"ui/?redirected&p={domain}/{project}&execution-report#/test-set-report/{runId}");
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (Exception e)
            {
                // result url will be NA (in case of failure like getting ALM version, convert ALM version to number)
                Console.WriteLine(e.Message);
            }
            return url;
        }

        public async Task<bool> IsNewReport(IClient client)
        {
            AlmVersion version = await GetAlmVersion(client);
            // Newer than 12.2x, including 12.5x, 15.x and later
            _ = int.TryParse(version.MajorVersion, out int majorVersion);
            _ = int.TryParse(version.MinorVersion, out int minorVersion);
            return (majorVersion == 12 && minorVersion >= 2) || majorVersion > 12;
        }

        private async Task<AlmVersion> GetAlmVersion(IClient client)
        {
            Response res = await new GetAlmVersionRequest(client).Execute();
            if (res.IsOK)
                return res.Data.DeserializeXML<AlmVersion>();
            else
                throw new AlmException($"Failed to get ALM version. HTTP status code: {res.StatusCode}", ErrorCategory.InvalidResult);
        }
    }
}