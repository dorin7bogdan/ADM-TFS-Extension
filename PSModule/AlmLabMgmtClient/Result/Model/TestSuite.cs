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