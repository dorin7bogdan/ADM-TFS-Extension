using PSModule.AlmLabMgmtClient.SDK.Interface;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public class EventLogRequest : GetRequest
    {
        private readonly string _suffix;

        public EventLogRequest(IClient client, string timeslotId) : base(client, timeslotId)
        {
            _suffix = $"event-log-reads?query={{context[\"*Timeslot: {timeslotId};*\"]}}&fields=id,event-type,creation-time,action,description";
        }

        protected override string Suffix => _suffix;
    }
}