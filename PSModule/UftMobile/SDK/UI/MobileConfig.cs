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


using PSModule.UftMobile.SDK.Entity;
using PSModule.UftMobile.SDK.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSModule.UftMobile.SDK.UI
{
    public class MobileConfig : ServerConfig
    {
        private readonly bool _useProxy;
        private readonly ProxyConfig _proxyConfig;
        private readonly Device _device;
        private readonly AppConfig _appConfig;
        private readonly string _workDir;
        private readonly List<App> _extraApps = new();

        public bool UseProxy => _useProxy;
        public ProxyConfig ProxyConfig => _proxyConfig;
        public Device Device => _device;
        public AppType AppType => _appConfig.AppType;
        public SysApp SysApp => _appConfig.SysApp;
        public AppLine AppLine => _appConfig.AppLine;
        public List<AppLine> ExtraAppLines => _appConfig.ExtraAppsLines;
        public AppAction AppAction => _appConfig.AppAction;
        public DeviceMetrics DeviceMetrics => _appConfig.DeviceMetrics;

        public string WorkDir => _workDir;
        public string MobileInfo { get; set; }

        public App App { get; set; }
        public List<App> ExtraApps { get { return _extraApps; } }

        public MobileConfig(ServerConfig srvConfig, bool useProxy, ProxyConfig proxyConfig, Device device = null, AppConfig appConfig = null, string workDir = ""): base(srvConfig)
        {
            _useProxy = useProxy;
            _proxyConfig = proxyConfig;
            _device = device;
            _appConfig = appConfig;
            _workDir = workDir;
        }
    }
}
