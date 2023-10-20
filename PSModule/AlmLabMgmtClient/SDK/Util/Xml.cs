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
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PSModule.AlmLabMgmtClient.SDK.Util
{
    public static class Xml
    {
        private const string FIELDS = "Fields";
        private const string FIELD = "Field";
        private const string NAME = "Name";
        private const string VALUE = "Value";
        private const string ENTITY = "Entity";
        private const string TOTAL_RESULTS = "TotalResults";

        public static string GetAttributeValue(string xml, string attrName)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                if (doc?.Root?.Element(FIELDS)?.HasElements == true)
                {
                    var fields = doc.Root.Element(FIELDS).Elements(FIELD);
                    foreach (XElement field in fields)
                    {
                        if (field.Attribute(NAME)?.Value == attrName)
                            return field.Element(VALUE)?.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return string.Empty;
        }
        public static IList<IDictionary<string, string>> ToEntities(string xml)
        {
            var list = new List<IDictionary<string, string>>();
            try
            {
                var doc = XDocument.Parse(xml);
                var entities = doc.Root.Elements(ENTITY);
                foreach (var entity in entities)
                {
                    var newEntity = new Dictionary<string, string>();
                    var fields = entity.Element(FIELDS).Elements(FIELD);
                    foreach (var field in fields)
                        if (field.HasAttributes)
                            newEntity.Add(field.Attributes().First().Value, field.Element(VALUE)?.Value);
                    list.Add(newEntity);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return list;
        }

        internal static bool HasResults(string xml)
        {
            bool ok = false;
            try
            {
                var doc = XDocument.Parse(xml);
                if (doc.Root.HasAttributes && int.TryParse(doc.Root.Attribute(TOTAL_RESULTS).Value, out int res))
                { 
                    ok = res > 0;
                }
                else if (doc.Root.HasElements && doc.Root.Elements(ENTITY).Any())
                {
                    ok = true;
                }
                return ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal static IList<int> GetTestSetIds(string xml)
        {
            var doc = XDocument.Parse(xml);
            var ids = new List<int>();

            doc.Root?.Elements("Entity")?.Elements("Fields")?.Elements("Field")?.
                Where(el => el?.Attribute("Name")?.Value == "cycle-id")?.Elements("Value")?.
                ForEach(v =>
                {
                    if (int.TryParse(v?.Value, out int id))
                        ids.Add(id);
                });
            return ids;
        }
    }
}
