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

using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.Common;
using System.Net;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Auth
{
    using C = Constants;
    public class ApiKeyAuthenticator : IAuthenticator
    {
        private const string APIKEY_LOGIN_API = "rest/oauth2/login";
        private const string ALM_CLIENT_TYPE = "ALM-CLIENT-TYPE";

        public async Task<bool> Login(IClient client)
        {
            string clientId = client.Credentials.UsernameOrClientID;
            string secret = client.Credentials.PasswordOrSecret;
            string clientType = client.ClientType;
            string body = $"{{clientId:{clientId}, secret:{secret}}}";
            var headers = new WebHeaderCollection
            {
                { ALM_CLIENT_TYPE, clientType },
                { HttpRequestHeader.ContentType, C.APP_JSON},
                { HttpRequestHeader.Accept, C.APP_JSON}
            };

            await client.Logger.LogInfo("Start login to ALM server with APIkey...");

            var res = await client.HttpPost(client.ServerUrl.AppendSuffix(APIKEY_LOGIN_API), headers, body);
            bool ok = res.IsOK;
            await client.Logger.LogInfo(ok ? $"Logged in successfully to ALM Server {client.ServerUrl}"
                                  : $"Login to ALM Server at {client.ServerUrl} failed. Status Code: [{res.StatusCode}]");
            return ok;
        }

        public async Task<bool> Logout(IClient client)
        {
            //No logout
            return await Task.FromResult(true);
        }
    }
}
