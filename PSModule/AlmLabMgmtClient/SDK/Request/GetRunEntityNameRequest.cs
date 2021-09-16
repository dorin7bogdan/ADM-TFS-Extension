using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Request;

namespace AlmLabMgmtClient.SDK.Request
{
    public class GetRunEntityNameRequest : GetRequest
    {
        private readonly string _nameSuffix;

        public GetRunEntityNameRequest(IClient client, string suffix, string entityId) : base(client, entityId)
        {
            _nameSuffix = suffix;
        }

        protected override string Suffix => _nameSuffix;
    }
}