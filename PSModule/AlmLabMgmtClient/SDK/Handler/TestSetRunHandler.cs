using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Handler
{
    public class TestSetRunHandler : RunHandler
    {
        public TestSetRunHandler(IClient client, string entityId) : base(client, entityId) { }

        protected override string StartSuffix => $"test-sets/{_entityId}/startruntestset";

        public override string NameSuffix => $"test-sets/{_entityId}";
    }
}
