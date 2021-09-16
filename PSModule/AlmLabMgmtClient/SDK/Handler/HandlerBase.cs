using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Handler
{
    public abstract class HandlerBase
    {
        protected readonly IClient _client;
        protected readonly ILogger _logger;
        protected readonly string _entityId;
        protected string _runId = string.Empty;
        protected string _timeslotId = string.Empty;

        protected HandlerBase(IClient client, string entityId)
        {
            _client = client;
            _entityId = entityId;
            _logger = _client.Logger;
        }

        protected HandlerBase(IClient client, string entityId, string runId) : this(client, entityId)
        {
            _runId = runId;
        }

        public string EntityId => _entityId;

        public string RunId => _runId;

        public void SetRunId(string value)
        {
            _runId = value;
        }
    }
}
