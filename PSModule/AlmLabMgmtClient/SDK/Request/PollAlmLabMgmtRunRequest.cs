using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public class PollAlmLabMgmtRunRequest : GetRequest
    {
        public PollAlmLabMgmtRunRequest(IClient client, string runId) : base(client, runId)
        {
        }

        protected override string Suffix => $"{PROC_RUNS}/{_runId}";
    }
}