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

using PSModule.UftMobile.SDK.Interface;
using PSModule.Common;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using PSModule.UftMobile.SDK.Entity;
using PSModule.UftMobile.SDK.Enums;

namespace PSModule.UftMobile.SDK.Auth
{
    using C = Constants;
    public class OAuth2Authenticator : IAuthenticator
    {
        private const string OAUTH2_CREDENTIALS = "rest/oauth2/credentials";
        public const string OAUTH2_LOGIN = "rest/oauth2/token";

        public async Task<bool> Login(IClient client)
        {
            dynamic body = new
            {
                client = client.Credentials.UsernameOrClientId,
                secret = client.Credentials.PasswordOrSecret,
                tenant = client.Credentials.TenantId
            };
            string jsonBody = JsonConvert.SerializeObject(body);

            var res = await client.HttpPost<AccessToken>(OAUTH2_LOGIN, jsonBody, resType: ResType.Object);
            client.IsLoggedIn = res.IsOK;
            if (res.IsOK)
            {
                client.AccessToken = res.Entity;
            }

            return res.IsOK;
        }

        public async Task<bool> Logout(IClient client)
        {
            //No logout
            return await Task.FromResult(true);
        }
    }
}
