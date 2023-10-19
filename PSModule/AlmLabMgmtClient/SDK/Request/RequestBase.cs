/*
 * Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
 * This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
 * Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
 * __________________________________________________________________
 * MIT License
 *
 * Copyright 2012-2023 Open Text
 *
 * The only warranties for products and services of Open Text and
 * its affiliates and licensors ("Open Text") are as may be set forth
 * in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or
 * omissions contained herein. The information contained herein is subject
 * to change without notice.
 *
 * Except as specifically indicated otherwise, this document contains
 * confidential information and a valid license is required for possession,
 * use or copying. If this work is provided to the U.S. Government,
 * consistent with FAR 12.211 and 12.212, Commercial Computer Software,
 * Computer Software Documentation, and Technical Data for Commercial Items are
 * licensed to the U.S. Government under vendor's standard commercial license.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
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