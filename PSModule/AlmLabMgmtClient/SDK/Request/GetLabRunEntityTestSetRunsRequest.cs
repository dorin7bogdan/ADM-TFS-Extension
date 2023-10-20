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
using PSModule.Common;
using System.Net;

namespace PSModule.AlmLabMgmtClient.SDK.Request
{
    using C = Constants;
    public class GetLabRunEntityTestSetRunsRequest : GetRequest
    {
        private const string PROCEDURE_TESTSET_INSTANCE_RUNS = "procedure-testset-instance-runs";
        public GetLabRunEntityTestSetRunsRequest(IClient client, string runId) : base(client, runId)
        {
        }

        protected override string Suffix => PROCEDURE_TESTSET_INSTANCE_RUNS;

        protected override string QueryString => $"query={{procedure-run[{_runId}]}}&page-size=2000&fields=test-subtype,start-exec-time,test-config-name,duration,start-exec-date,testset-name,testcycl-id,status,run-id";

        // It's pretty weird that in 1260 p1 the xml header should be provided. Otherwise the server would generate wrong query sql.
        protected override WebHeaderCollection Headers =>
            new()
            {
                { HttpRequestHeader.ContentType, C.APP_XML },
                { HttpRequestHeader.Accept, C.APP_XML }
            };

}
}