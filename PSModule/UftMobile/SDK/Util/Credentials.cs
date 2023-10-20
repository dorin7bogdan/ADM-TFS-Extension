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

using PSModule.Properties;
using PSModule.UftMobile.SDK.Enums;
using System.Management.Automation;

namespace PSModule.UftMobile.SDK.Util
{
    public class Credentials
    {
        private readonly string _usernameOrClientId;
        private readonly string _passwordOrSecret;
        private readonly int _tenantId;

        public string UsernameOrClientId => _usernameOrClientId;
        public string PasswordOrSecret => _passwordOrSecret;
        public int TenantId => _tenantId;

        public Credentials(string usernameorClientId, string passwordOrSecret, int tenantId = 0)
        {
            if (usernameorClientId.IsNullOrWhiteSpace())
                throw new UftMobileException(Resources.MissingUsernameOrClientId, ErrorCategory.InvalidArgument);

            _usernameOrClientId = usernameorClientId;
            _passwordOrSecret = passwordOrSecret;
            _tenantId = tenantId;
        }
    }
}
