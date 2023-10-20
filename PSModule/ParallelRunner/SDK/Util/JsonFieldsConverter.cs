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

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace PSModule.ParallelRunner.SDK.Util
{
    public class JsonFieldsConverter : JsonPathConverter
    {
        public override object ReadJson(JsonReader reader, Type objType, object existingValue, JsonSerializer serializer)
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
            object targetObj = existingValue ?? Activator.CreateInstance(objType);

            foreach (var field in objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                var pathAttr = field.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();

                string jsonPath = pathAttr?.PropertyName ?? field.Name;
                var token = jo.SelectToken(jsonPath);

                if (token != null && token.Type != JTokenType.Null)
                {
                    object value = token.ToObject(field.FieldType, serializer);
                    field.SetValue(targetObj, value);
                }
            }

            return targetObj;
        }

        /// <remarks>
        /// CanConvert is not called when <see cref="JsonConverterAttribute">JsonConverterAttribute</see> is used.
        /// </remarks>
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetCustomAttributes(true).OfType<JsonFieldsConverter>().Any();
        }
    }
}
