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
