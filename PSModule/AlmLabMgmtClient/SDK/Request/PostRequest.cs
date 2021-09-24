using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public abstract class PostRequest : PostRequestBase
    {
        protected readonly string _runId;
        protected PostRequest(IClient client, string runId) : base(client)
        {
            _runId = runId;
        }
    }
}
