using AlmLabMgmtClient.SDK.Request;
using PSModule.AlmLabMgmtClient.SDK;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Request;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.Result
{
    public class LabPublisher : Publisher
    {
        private const string NAME = "name";
        public LabPublisher(IClient client, string entityId, string runId, string nameSuffix) : base(client, entityId, runId, nameSuffix)
        {
        }

        protected async override Task<string> GetEntityName()
        {
            string name = "Unnamed Entity";
            try
            {
                Response response = await GetRunEntityName();
                if (response.IsOK && !response.Data.IsNullOrWhiteSpace())
                {
                    name = Xml.GetAttributeValue(response.Data, NAME);
                }
                else
                {
                    _logger.LogError($"Failed to get Entity name. Exception: {response.Error}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return name;
        }

        protected override GetRequest GetRunEntityTestSetRunsRequest(IClient client, string runId)
        {
            return new GetLabRunEntityTestSetRunsRequest(_client, _runId);
        }
    }
}