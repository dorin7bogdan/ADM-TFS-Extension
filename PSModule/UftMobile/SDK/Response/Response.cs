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
using PSModule.UftMobile.SDK.Enums;
using System;
using System.Net;

namespace PSModule.UftMobile.SDK
{
    public class Response
    {
        protected string _error;
        protected readonly WebHeaderCollection _headers;
        protected readonly HttpStatusCode? _statusCode;

        public WebHeaderCollection Headers => _headers;
        public string Error => _error;
        public HttpStatusCode? StatusCode => _statusCode;

        protected Response()
        {
        }

        public Response(string err, HttpStatusCode? statusCode = null)
        {
            _error = err;
            _statusCode = statusCode;
        }
        public Response(string body, WebHeaderCollection headers, HttpStatusCode statusCode) : this(headers, statusCode)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<Result>(body);
                if (res.Error)
                {
                    _error = res.Message;
                }
            }
            catch
            {
                bool.TryParse(body, out bool ok);
                if (!ok)
                {
                    _error = body;
                }
            }
        }

        public Response(WebHeaderCollection headers, HttpStatusCode statusCode)
        {
            _headers = headers;
            _statusCode = statusCode;
        }
        public bool IsOK => _error.IsNullOrWhiteSpace() && _statusCode.In(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted);

    }

    public class Response<T> : Response where T : class
    {
        private readonly T[] _entries;
        public T[] Entities => _entries ?? new T[0];
        public T Entity => _entries?[0];
        public Response(string body, WebHeaderCollection headers, HttpStatusCode statusCode, ResType resType) : base(headers, statusCode)
        {
            switch (resType)
            {
                case ResType.DataEntities:
                    {
                        var res = JsonConvert.DeserializeObject<MultiResult<T>>(body);
                        _entries = res.Entries;
                        if (res.Error)
                        {
                            _error = res.Message;
                        }
                        break;
                    }
                case ResType.DataEntity:
                    {
                        var res = JsonConvert.DeserializeObject<SingleResult<T>>(body);
                        _entries = new T[] { res.Entry };
                        if (res.Error)
                        {
                            _error = res.Message;
                        }
                        break;
                    }
                case ResType.Array:
                    {
                        try
                        {
                            Result res = JsonConvert.DeserializeObject<Result>(body);
                            if (res.Error)
                            {
                                _error = res.Message;
                            }
                        }
                        catch { /*  no action */ }
                        finally
                        {
                            if (_error.IsNullOrEmpty())
                            {
                                _entries = JsonConvert.DeserializeObject<T[]>(body);
                            }
                        }
                        break;
                    }
                default:
                    {
                        try
                        {
                            Result res = JsonConvert.DeserializeObject<Result>(body);
                            if (res.Error)
                            {
                                _error = res.Message;
                            }
                        }
                        catch { /*  no action */ }
                        finally
                        {
                            if (_error.IsNullOrEmpty())
                            {
                                T obj = JsonConvert.DeserializeObject<T>(body);
                                _entries = new T[] { obj };
                            }
                        }
                        break;
                    }
            }
        }
        public Response(string err, HttpStatusCode? statusCode = null) : base(err, statusCode)
        {
        }
    }
}
