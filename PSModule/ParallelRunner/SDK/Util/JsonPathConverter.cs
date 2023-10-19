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
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace PSModule.ParallelRunner.SDK.Util
{
    /// <summary>
    /// JsonPathConverter; adapted from code located at: https://stackoverflow.com/a/33094930/398630
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class JsonPathConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo;
            try
            {
                jo = JObject.Load(reader);
            }
            catch
            {
                return null;
            }
            object targetObj = existingValue ?? Activator.CreateInstance(objectType);

            foreach (var prop in objectType.GetProperties().Where(p => p.CanRead))
            {
                var pathAttribute = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
                var converterAttribute = prop.GetCustomAttributes(true).OfType<JsonConverterAttribute>().FirstOrDefault();

                string jsonPath = pathAttribute?.PropertyName ?? prop.Name;
                var token = jo.SelectToken(jsonPath);

                if (token != null && token.Type != JTokenType.Null)
                {
                    bool done = false;

                    if (converterAttribute != null)
                    {
                        var args = converterAttribute.ConverterParameters ?? new object[0];
                        if (Activator.CreateInstance(converterAttribute.ConverterType, args) is JsonConverter converter && converter.CanRead)
                        {
                            using var sr = new StringReader(token.ToString());
                            using var jr = new JsonTextReader(sr);
                            var value = converter.ReadJson(jr, prop.PropertyType, prop.GetValue(targetObj), serializer);
                            if (prop.CanWrite)
                            {
                                prop.SetValue(targetObj, value);
                            }
                            done = true;
                        }
                    }

                    if (!done)
                    {
                        if (prop.CanWrite)
                        {
                            object value = token.ToObject(prop.PropertyType, serializer);
                            prop.SetValue(targetObj, value);
                        }
                        else
                        {
                            using StringReader sr = new(token.ToString());
                            serializer.Populate(sr, prop.GetValue(targetObj));
                        }
                    }
                }
            }

            return targetObj;
        }

        /// <remarks>
        /// CanConvert is not called when <see cref="JsonConverterAttribute">JsonConverterAttribute</see> is used.
        /// </remarks>
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetCustomAttributes(true).OfType<JsonPathConverter>().Any();
        }
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
