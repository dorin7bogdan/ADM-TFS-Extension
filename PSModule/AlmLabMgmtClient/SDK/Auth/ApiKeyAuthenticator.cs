using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Auth
{
    using C = Constants;
    public class ApiKeyAuthenticator : IAuthenticator
    {
        private const string APIKEY_LOGIN_API = "rest/oauth2/login";
        private const string ALM_CLIENT_TYPE = "ALM-CLIENT-TYPE";

        [SuppressMessage("Critical Code Smell", "S927:Parameter names should match base declaration and other partial definitions", Justification = "<clientId> and <secret> are more suggestive in this case")]
        public async Task<bool> Login(IClient client, string clientId, string secret, string clientType)
        {
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
            await client.Logger.LogInfo(ok ? $"Logged in successfully to ALM Server {client.ServerUrl} using clientId [{clientId}]"
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
