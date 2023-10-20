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