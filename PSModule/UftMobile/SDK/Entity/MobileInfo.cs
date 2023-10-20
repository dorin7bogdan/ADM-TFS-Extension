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
