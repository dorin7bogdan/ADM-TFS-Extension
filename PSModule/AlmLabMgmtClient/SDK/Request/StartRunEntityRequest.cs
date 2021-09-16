using PSModule.AlmLabMgmtClient.SDK.Interface;
using System.Collections.Generic;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public class StartRunEntityRequest : PostRequest
    {
        private const string DURATION = "duration";
        private const string VUDS_MODE = "vudsMode";
        private const string RESERVATION_ID = "reservationId";
        private const string MINUS_ONE = "-1";
        private const string VALUE_SET_ID = "valueSetId";

        private readonly string _duration;
        private readonly string _suffix;
        private readonly string _envConfigId;

        public StartRunEntityRequest(IClient client, string suffix, string runId, string duration, string envConfigId) : base(client, runId)
        {
            _duration = duration;
            _suffix = suffix;
            _envConfigId = envConfigId;
        }

        protected override IList<KeyValuePair<string, string>> DataFields => GetDataFields();

        private IList<KeyValuePair<string, string>> GetDataFields()
        {
            var fields = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(DURATION, _duration),
                new KeyValuePair<string, string>(VUDS_MODE, bool.FalseString.ToLower()),
                new KeyValuePair<string, string>(RESERVATION_ID, MINUS_ONE),
            };
            if (!_envConfigId.IsNullOrWhiteSpace())
            {
                fields.Add(new KeyValuePair<string, string>(VALUE_SET_ID, _envConfigId));
            }

            return fields;
        }

        protected override string Suffix => _suffix;

    }
}