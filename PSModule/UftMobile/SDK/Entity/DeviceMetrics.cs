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

using Newtonsoft.Json;
using C = PSModule.Common.Constants;

namespace PSModule.UftMobile.SDK.Entity
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class DeviceMetrics
    {
        private bool cpu;
		private bool memory;
        private bool freeMemory;
        private bool logs;
        private bool wifiState;
        private bool thermalState;
        private bool freeDiskSpace;
        //private bool screenshot;
        //private bool wifiSignalStrength;
        //private bool totalDiskSpace;

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
    }
}
