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

using System.Net;

namespace PSModule.AlmLabMgmtClient.SDK
{
    public class Response
    {
        private readonly WebHeaderCollection _headers;
        private readonly string _data;
        private readonly string _error;
        private readonly HttpStatusCode? _statusCode;
        public WebHeaderCollection Headers => _headers;
        public string Data => _data;
        public string Error => _error;
        public HttpStatusCode? StatusCode => _statusCode;

        public Response()
        {
        }

        public Response(string err, HttpStatusCode? statusCode = null)
        {
            _error = err;
            _statusCode = statusCode;
        }

        public Response(string data, WebHeaderCollection headers, HttpStatusCode statusCode)
        {
            _headers = headers;
            _data = data;
            _statusCode = statusCode;
        }

        public Response(WebHeaderCollection headers, HttpStatusCode statusCode)
        {
            _headers = headers;
            _statusCode = statusCode;
        }
        public bool IsOK => _error == null && _statusCode.In(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted);

        public override string ToString()
        {
            return _data;
            //return Encoding.UTF8.GetString(Data);
        }

    }
}
