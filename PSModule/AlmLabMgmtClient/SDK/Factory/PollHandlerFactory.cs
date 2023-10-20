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

using PSModule.AlmLabMgmtClient.SDK.Handler;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using PSModule.Common;
using System.Management.Automation;

namespace PSModule.AlmLabMgmtClient.SDK.Factory
{
    using C = Constants;
    public class PollHandlerFactory
    {
        public PollHandler Create(IClient client, string runType, string entityId)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type", ErrorCategory.InvalidType);
            }

            return ret;
        }

        public PollHandler Create(IClient client, string runType, string entityId, int interval)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId, interval);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type", ErrorCategory.InvalidType);
            }

            return ret;
        }
    }
}