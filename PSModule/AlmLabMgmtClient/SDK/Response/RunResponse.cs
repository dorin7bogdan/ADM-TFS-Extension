/*
 * Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
 * This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
 * Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
 * __________________________________________________________________
 * MIT License
 *
 * Copyright 2012-2023 Open Text
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

using PSModule.AlmLabMgmtClient.SDK.Util;
using PSModule.Common;

namespace PSModule.AlmLabMgmtClient.SDK
{
    using C = Constants;
    public class RunResponse
    {
        private const string SUCCESS_STATUS = "SuccessStaus";
        private const string INFO = "info";

        private string _successStatus;
        private string _runId;

        public void Initialize(Response response)
        {
            string xml = response.ToString();
            _successStatus = Xml.GetAttributeValue(xml, SUCCESS_STATUS);
            _runId = ParseRunId(Xml.GetAttributeValue(xml, INFO));
        }

        protected string ParseRunId(string runIdResponse)
        {
            return runIdResponse.IsNullOrWhiteSpace() ? C.NO_RUN_ID : runIdResponse;
        }

        public string RunId => _runId;

        public bool HasSucceeded => _successStatus == C.ONE;

    }
}