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
 * Except as specifically indicated otherwise, this document contains
 * confidential information and a valid license is required for possession,
 * use or copying. If this work is provided to the U.S. Government,
 * consistent with FAR 12.211 and 12.212, Commercial Computer Software,
 * Computer Software Documentation, and Technical Data for Commercial Items are
 * licensed to the U.S. Government under vendor's standard commercial license.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
 */

using PSModule.AlmLabMgmtClient.SDK;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Request;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.Result
{
    public class LabPublisher : Publisher
    {
        private const string NAME = "name";
        public LabPublisher(IClient client, string entityId, string runId, string nameSuffix) : base(client, entityId, runId, nameSuffix)
        {
        }

        protected async override Task<string> GetEntityName()
        {
            string name = "Unnamed Entity";
            try
            {
                Response response = await GetRunEntityName();
                if (response.IsOK && !response.Data.IsNullOrWhiteSpace())
                {
                    name = Xml.GetAttributeValue(response.Data, NAME);
                }
                else
                {
                    await _logger.LogError($"Failed to get Entity name. Exception: {response.Error}");
                }
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (Exception e)
            {
                await _logger.LogError(e.Message);
            }

            return name;
        }

        protected override GetRequest GetRunEntityTestSetRunsRequest(IClient client, string runId)
        {
            return new GetLabRunEntityTestSetRunsRequest(_client, _runId);
        }
    }
}