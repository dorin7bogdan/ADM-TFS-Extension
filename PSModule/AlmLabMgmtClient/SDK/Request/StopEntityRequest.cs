using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public class StopEntityRequest : PostRequest
    {
        private const string STOP = "stop";
        public StopEntityRequest(IClient client, string runId) : base(client, runId) { }
        protected override string Suffix => $"{PROC_RUNS}/{_runId}/{STOP}";
    }
}