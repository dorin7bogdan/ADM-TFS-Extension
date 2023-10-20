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

using System;

namespace PSModule.Models
{
    public class MobileSettings
    {
        public bool useMC;
        public String mcServerUrl;// = map.get(RunFromFileSystemTaskConfigurator.MCSERVERURL);
        public String mcUserName;// = map.get(RunFromFileSystemTaskConfigurator.MCUSERNAME);
        public String mcPassword;// = map.get(RunFromFileSystemTaskConfigurator.MCPASSWORD);
        public String proxyAddress;// = null;
        public String proxyUserName;// = null;
        public String proxyPassword;// = null;

        public bool useSSL;
        public bool useProxy;

        public bool specifyAuthentication;
    }
}
