
namespace PSModule.UftMobile.SDK.UI
{
    public class DeviceMetrics
    {
		private bool cpu;
		private bool memory;
        private bool freeMemory;
        private bool logs;
        private bool wifiState;
        private bool thermalState;
        private bool freeDiskSpace;

        public DeviceMetrics(bool cpu, bool memory, bool freeMemory, bool logs, bool wifiState, bool thermalState, bool freeDiskSpace)
        {
            this.cpu = cpu;
            this.memory = memory;
            this.freeMemory = freeMemory;
            this.logs = logs;
            this.wifiState = wifiState;
            this.thermalState = thermalState;
            this.freeDiskSpace = freeDiskSpace;
        }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
