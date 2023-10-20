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


using Newtonsoft.Json;
using System.Collections.Generic;
using C = PSModule.Common.Constants;
namespace PSModule.UftMobile.SDK.Entity
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class MobileInfo
    {
        private static readonly char[] _escapeChars = new char[] { C.BACK_SLASH, C.COLON };

        private readonly string id;
        private readonly App application;
        private readonly List<Device> devices;
        private readonly string header;
        private readonly CapableDeviceFilterDetails capableDeviceFilterDetails;
        private List<App> extraApps;

        public MobileInfo(string jobId, Device device = null, CapableDeviceFilterDetails cdfDetails = null, App app = null, List<App> extraApps = null, string hdr = null)
        {
            id = jobId;
            devices = device == null ? null : new() { device };
            capableDeviceFilterDetails = cdfDetails;
            application = app;
            this.extraApps = extraApps;
            header = hdr;
        }

        public override string ToString()
        {
            return this.ToJson(_escapeChars);
        }
    }
}
