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

using PSModule.AlmLabMgmtClient.SDK.Interface;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    public abstract class RequestBase : IRequest
    {
        protected readonly IClient _client;
        protected readonly ILogger _logger;
        protected const string X_XSRF_TOKEN = "X-XSRF-TOKEN";
        protected const string PROC_RUNS = "procedure-runs";

        protected RequestBase(IClient client)
        {
            _client = client;
            _logger = _client.Logger;
        }

        public async Task<Response> Execute(bool logRequestUrl = true)
        {
            Response res;
            try
            {
                res = await Perform(logRequestUrl);
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogError(ex.Message);
                res = new Response(ex.Message);
            }

            return res;
        }

        public abstract Task<Response> Perform(bool logRequestUrl = true);

        protected virtual string Suffix => null;

        protected virtual WebHeaderCollection Headers => new WebHeaderCollection { { X_XSRF_TOKEN, _client.XsrfTokenValue } };

        protected string Body => null;

        protected virtual string Url => _client.BuildRestEndpoint(Suffix);
    }
}