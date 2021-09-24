using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public abstract class GetRequest : GetRequestBase
    {
        protected readonly string _runId;
        protected GetRequest(IClient client, string runId) : base(client)
        {
            _runId = runId;
        }
    }
}
