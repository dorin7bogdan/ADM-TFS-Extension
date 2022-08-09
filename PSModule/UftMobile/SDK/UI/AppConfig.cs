
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
        private List<AppLine> _apps;
        private bool _install;
        private bool _restart;
        private bool _uninstall;
        private DeviceMetrics _deviceMetrics;

        public AppType AppType => _appType;
        public SysApp SysApp => _sysApp;
        public List<AppLine> AppLines => _apps;
        public bool Install => _install;
        public bool Restart => _restart;
        public bool Uninstall => _uninstall;
        public DeviceMetrics DeviceMetrics => _deviceMetrics;

        public AppConfig(string appType, string sysApp, List<AppLine> apps, DeviceMetrics deviceMetrics, bool install, bool restart, bool uninstall)
        {
            Enum.TryParse(appType, true, out _appType);
            Enum.TryParse(sysApp, true, out _sysApp);
            _apps = apps;
            _install = install;
            _restart = restart;
            _uninstall = uninstall;
            _deviceMetrics = deviceMetrics;
        }
    }
}
