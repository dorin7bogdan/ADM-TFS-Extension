/*
 * Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
 * This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
 * Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
 * __________________________________________________________________
 * MIT License
 *
 * Copyright 2016-2023 Open Text
 *
 * The only warranties for products and services of Open Text and
 * its affiliates and licensors ("Open Text") are as may be set forth
 * in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or
 * omissions contained herein. The information contained herein is subject
 * to change without notice.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
 */

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