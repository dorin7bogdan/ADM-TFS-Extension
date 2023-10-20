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

namespace PSModule.UftMobile.SDK.UI
{
    public class AppConfig
    {
        private AppType _appType;
        private SysApp _sysApp;
        private AppLine _app;
        private List<AppLine> _apps;
        private AppAction _appAction;
        private DeviceMetrics _deviceMetrics;

        public AppType AppType => _appType;
        public SysApp SysApp => _sysApp;
        public AppLine AppLine => _app;
        public List<AppLine> ExtraAppsLines => _apps ?? new();
        public AppAction AppAction => _appAction;
        public DeviceMetrics DeviceMetrics => _deviceMetrics;

        public AppConfig(string appType, string sysApp, AppLine app, List<AppLine> apps, DeviceMetrics deviceMetrics, AppAction appAction)
        {
            Enum.TryParse(appType, true, out _appType);
            Enum.TryParse(sysApp, true, out _sysApp);
            _app = app;
            _apps = apps;
            _appAction = appAction;
            _deviceMetrics = deviceMetrics;
        }
    }
}
