using System.Xml.Serialization;
/**
* <p>
* Java class for anonymous complex type.
* 
* <p>
* The following schema fragment specifies the expected content contained within this class.
* 
* <pre>
* <complexType>
*   <complexContent>
*     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
*       <attribute name="type" type="{http://www.w3.org/2001/XMLSchema}string" />
*       <attribute name="message" type="{http://www.w3.org/2001/XMLSchema}string" />
*     </restriction>
*   </complexContent>
* </complexType>
* </pre>
* 
* 
*/
namespace PSModule.AlmLabMgmtClient.Result.Model
{
    [XmlRoot(ElementName = "error")]
    public class Error
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }
    }
}