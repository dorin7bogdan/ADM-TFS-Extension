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

using PSModule.UftMobile.SDK.Interface;
using PSModule.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.UftMobile.SDK.Auth
{
    public class BasicAuthenticator : IAuthenticator
    {
        private const string LOGIN_ENDPOINT = "rest/client/login";
        private const string LOGOUT_ENDPOINT = "rest/client/logout";

        public async Task<bool> Login(IClient client)
        {
            string username = client.Credentials.UsernameOrClientId;
            string pass = client.Credentials.PasswordOrSecret;

            try
            {
                string body = @$"{{""name"":""{username}"", ""password"":""{pass}"", ""accountName"": ""default""}}";
                var res = await client.HttpPost(LOGIN_ENDPOINT, body: body);
                if (!res.IsOK)
                {
                    await client.Logger.LogDebug($"StatusCode=[{res.StatusCode}]");
                    await client.Logger.LogInfo($"{res.Error}");
                }
                client.IsLoggedIn = res.IsOK;
                return res.IsOK;
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (Exception e)
            {
                await client.Logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> Logout(IClient client)
        {
            bool isLoggedOut = false;
            if (client != null)
            {
                string body = @"{{ ""data"": {{ }}, ""error"": false, ""message"": ""string"", ""messageCode"": 0 }}";
                Response response = await client.HttpPost(LOGOUT_ENDPOINT, body: body);
                isLoggedOut = response.IsOK;
            }
            return isLoggedOut;
        }
    }
}
