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
