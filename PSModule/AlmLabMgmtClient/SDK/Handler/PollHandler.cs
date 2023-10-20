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
 *  * Permission is hereby granted, free of charge, to any person obtaining a copy
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

using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Handler
{
    public abstract class PollHandler : HandlerBase
    {
        private readonly int _interval = 5000; // millisecond
        protected PollHandler(IClient client, string entityId) : base(client, entityId)
        {
        }

        protected PollHandler(IClient client, string entityId, int interval) : base(client, entityId)
        {
            // NOTE: this constructor is not used at this moment, but for safety, restrict the polling interval between 1 and 60 seconds
            if (interval < 1000 || interval > 60000)
                throw new AlmException($"PollHandler: Invalid polling interval : {interval} milliseconds. Between 1000 and 60000 milliseconds", ErrorCategory.LimitsExceeded);
            _interval = interval;
        }

        protected PollHandler(IClient client, string entityId, string runId) : base(client, entityId, runId)
        {
        }

        public async Task<bool> Poll()
        {
            await _logger.LogInfo($"Start Polling... Run ID: {_runId}");
            return await DoPoll();
        }

        protected virtual async Task<bool> DoPoll()
        {
            bool ok = false, logRequestUrl = true;

            int failures = 0;
            while (failures < 3)
            {
                var res = await GetResponse(logRequestUrl);
                if (res.IsOK)
                {
                    await LogProgress(logRequestUrl);
                    if (IsFinished(res))
                    {
                        ok = true;
                        LogRunEntityResults(await GetRunEntityResultsResponse());
                        break;
                    }
                    else
                    {
                        if (logRequestUrl)
                            await _logger.ShowProgress();
                        logRequestUrl = false;
                    }
                }
                else
                {
                    LogPollingError(res);
                    ++failures;
                }
                Sleep();
            }

            return ok;
        }

        protected abstract Task<Response> GetRunEntityResultsResponse();

        protected abstract bool LogRunEntityResults(Response response);

        protected abstract bool IsFinished(Response response);

        protected abstract Task<Response> GetResponse(bool logRequestUrl);

        protected void Sleep()
        {
            try
            {
                Thread.Sleep(_interval);
            }
            catch (ThreadInterruptedException)
            {
                _logger.LogError("Interrupted while polling");
                throw;
            }
        }

        protected void LogPollingError(Response res)
        {
            string err = "Polling try failed. ";
            if (res.StatusCode != null)
            {
                err += $"Status code: {res.StatusCode}, ";
            }
            err += $"Error: {res.Error ?? "Not Available"}";
            _logger.LogError(err);
        }

        protected abstract Task LogProgress(bool logRequestUrl);

    }
}