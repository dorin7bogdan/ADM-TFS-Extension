
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
        private readonly List<Tuple<App, bool>> _extraApps = new();

        public bool UseProxy => _useProxy;
        public ProxyConfig ProxyConfig => _proxyConfig;
        public Device Device => _device;
        public AppType AppType => _appConfig.AppType;
        public SysApp SysApp => _appConfig.SysApp;
        public AppLine MainAppLine => _appConfig.AppLines[0];
        public IEnumerable<AppLine> ExtraAppLines => _appConfig.AppLines.Skip(1);
        public bool Install => _appConfig.Install;
        public bool Restart => _appConfig.Restart;
        public bool Uninstall => _appConfig.Uninstall;
        public DeviceMetrics DeviceMetrics => _appConfig.DeviceMetrics;

        public string WorkDir => _workDir;
        public string MobileInfo { get; set; }

        public Tuple<App, bool> App { get; set; }
        public List<Tuple<App, bool>> ExtraApps { get { return _extraApps; } }

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
