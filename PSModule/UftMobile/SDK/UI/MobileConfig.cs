/*
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
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
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
