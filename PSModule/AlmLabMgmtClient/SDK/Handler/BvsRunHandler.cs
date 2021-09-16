using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Handler
{
    public class BvsRunHandler : RunHandler
    {
        public BvsRunHandler(IClient client, string entityId) : base(client, entityId) { }

        protected override string StartSuffix => $"procedures/{_entityId}/startrunprocedure";

        public override string NameSuffix => $"procedures/{_entityId}";
    }
}
