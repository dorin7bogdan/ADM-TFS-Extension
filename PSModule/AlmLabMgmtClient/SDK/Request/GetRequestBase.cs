using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public abstract class GetRequestBase : RequestBase
    {
        protected GetRequestBase(IClient client) : base(client)
        {
        }

        protected virtual string QueryString => null;

        public async override Task<Response> Perform(bool logRequestUrl = true)
        {
            return await _client.HttpGet(
                    Url,
                    Headers,
                    ResourceAccessLevel.PROTECTED,
                    QueryString,
                    logRequestUrl);
        }
    }
}