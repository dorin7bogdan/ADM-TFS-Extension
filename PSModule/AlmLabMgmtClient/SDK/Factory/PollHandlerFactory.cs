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

using PSModule.AlmLabMgmtClient.SDK.Handler;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using PSModule.Common;
using System.Management.Automation;

namespace PSModule.AlmLabMgmtClient.SDK.Factory
{
    using C = Constants;
    public class PollHandlerFactory
    {
        public PollHandler Create(IClient client, string runType, string entityId)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type", ErrorCategory.InvalidType);
            }

            return ret;
        }

        public PollHandler Create(IClient client, string runType, string entityId, int interval)
        {
            PollHandler ret;
            if (runType.In(C.BVS, C.TEST_SET))
            {
                ret = new LabPollHandler(client, entityId, interval);
            }
            else
            {
                throw new AlmException("PollHandlerFactory: Unrecognized run type", ErrorCategory.InvalidType);
            }

            return ret;
        }
    }
}