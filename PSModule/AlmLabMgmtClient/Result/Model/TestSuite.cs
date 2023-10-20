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

using System.Collections.Generic;
using System.Xml.Serialization;
/**
 * The following schema fragment specifies the expected content contained within this class.
 * 
 * <complexType>
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element ref="{}properties" minOccurs="0"/>
 *         <element ref="{}testcase" maxOccurs="unbounded" minOccurs="0"/>
 *         <element ref="{}system-out" minOccurs="0"/>
 *         <element ref="{}system-err" minOccurs="0"/>
 *       </sequence>
 *       <attribute name="name" use="required" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="tests" use="required" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="failures" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="errors" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="time" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="disabled" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="skipped" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="timestamp" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="hostname" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="id" type="{http://www.w3.org/2001/XMLSchema}string" />
 *       <attribute name="package" type="{http://www.w3.org/2001/XMLSchema}string" />
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * 
 */
namespace PSModule.AlmLabMgmtClient.Result.Model
{
    [XmlRoot(ElementName = "testsuite")]
    public class TestSuite
    {
        [XmlElement(ElementName = "properties")]
        public Properties Properties { get; set; }

        [XmlElement(ElementName = "testcase")]
        public List<TestCase> ListOfTestCases { get; set; } = new List<TestCase>();

        [XmlElement(ElementName = "system-out")]
        public string SystemOut { get; set; }
        
        [XmlElement(ElementName = "system-err")]
        public string SystemErr { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "tests")]
        public string Tests { get; set; }
        
        [XmlAttribute(AttributeName = "failures")]
        public string Failures { get; set; }
        
        [XmlAttribute(AttributeName = "errors")]
        public string Errors { get; set; }
        
        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
        
        [XmlAttribute(AttributeName = "disabled")]
        public string Disabled { get; set; }
        
        [XmlAttribute(AttributeName = "skipped")]
        public string Skipped { get; set; }
        
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        
        [XmlAttribute(AttributeName = "hostname")]
        public string Hostname { get; set; }
        
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        
        [XmlAttribute(AttributeName = "package")]
        public string Package { get; set; }

    }
}