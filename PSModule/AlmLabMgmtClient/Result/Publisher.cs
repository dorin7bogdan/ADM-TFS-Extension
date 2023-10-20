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
using PSModule.AlmLabMgmtClient.SDK.Handler;
using PSModule.AlmLabMgmtClient.SDK.Request;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PSModule.AlmLabMgmtClient.Result.Model;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System;
using PSModule.AlmLabMgmtClient.SDK;
using AlmLabMgmtClient.SDK.Request;
using AlmLabMgmtClient.SDK.Util;
using System.Threading;

namespace PSModule.AlmLabMgmtClient.Result
{
    public abstract class Publisher : HandlerBase
    {
        protected readonly string _nameSuffix;
        protected Publisher(IClient client, string entityId, string runId, string nameSuffix) : base(client, entityId, runId)
        {
            _nameSuffix = nameSuffix;
        }

        public async Task<TestSuites> Publish(string url, string domain, string project)
        {
            TestSuites testSuites = null;
            GetRequest testSetRunsRequest = GetRunEntityTestSetRunsRequest(_client, _runId);
            Response response = await testSetRunsRequest.Execute();
            var testInstanceRun = GetTestInstanceRun(response);
            string entityName = await GetEntityName();
            if (testInstanceRun?.Any() == true)
            {
                testSuites = new JUnitParser(_entityId).ToModel(testInstanceRun, entityName, url, domain, project);
            }

            return testSuites;
        }

        protected async Task<Response> GetRunEntityName()
        {
            return await new GetRunEntityNameRequest(_client, _nameSuffix, _entityId).Execute();
        }

        protected IList<IDictionary<string, string>> GetTestInstanceRun(Response response)
        {
            IList<IDictionary<string, string>> ret = null;
            try
            {
                if (!response.Data.IsNullOrWhiteSpace())
                {
                    ret = Xml.ToEntities($"{response}");
                }

                if (ret.IsNullOrEmpty())
                {
                    _logger.LogInfo($"Parse TestInstanceRuns from response XML got no result. Response: {response}");
                }
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to parse TestInstanceRuns response XML. Exception: {ex.Message}, XML: {response}");
            }

            return ret;
        }

        protected abstract GetRequest GetRunEntityTestSetRunsRequest(IClient client, string runId);
        protected abstract Task<string> GetEntityName();
    }
}