using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public class GetLabRunEntityDataRequest : GetRequest
    {
        public GetLabRunEntityDataRequest(IClient client, string runId) : base(client, runId) { }

        protected override string Suffix => $"{PROC_RUNS}/{_runId}";
    }
}