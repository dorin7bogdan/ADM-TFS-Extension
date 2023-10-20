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
