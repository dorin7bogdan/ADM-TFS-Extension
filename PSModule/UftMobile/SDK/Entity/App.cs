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

using Newtonsoft.Json;
using C = PSModule.Common.Constants;

namespace PSModule.UftMobile.SDK.Entity
{
    public class App
    {
        private const string ANY = "ANY";
        private const string MC_HOME = "MC.Home";
        private const string HOME = "Home";
        public string Type { get; set; } = ANY;

        public string Id { get; set; } = MC_HOME;

        public string Name { get; set; } = HOME;

        public string Version { get; set; }

        [JsonIgnore]
        public string FileName { get; set; }

        public string Identifier { get; set; } = MC_HOME;

        public bool Instrumented { get; set; }

        public string AppLocalPath { get; set; }

        public string UrlScheme { get; set; }

        [JsonIgnore]
        public string Icon { get; set; }

        [JsonIgnore]
        public bool ApplicationExists { get; set; }

        [JsonIgnore]
        public bool InstrumentedApplicationExists { get; set; }

        public int Counter { get; set; }

        [JsonIgnore]
        public string AppVersion { get; set; }

        [JsonIgnore]
        public string AppBuildVersion { get; set; }

        public string Source { get; set; }

        [JsonIgnore]
        public Workspace[] Workspaces { get; set; }

        public override string ToString()
        {
            return @$"Name: ""{Name}"", Identifier: ""{Identifier}"", Version: ""{Version}"", Type: ""{Type}"", Source: ""{Source}""";
        }

        public App() { }

        [JsonIgnore]
        public string Json4JobUpdate => @$"{{""type"":""{Type}"",""identifier"":""{Identifier}"",""instrumented"":{(Instrumented ? C.TRUE : C.FALSE)}}}";
    }
}
