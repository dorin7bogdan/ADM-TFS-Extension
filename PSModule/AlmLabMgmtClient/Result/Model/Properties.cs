using System.Collections.Generic;
using System.Xml.Serialization;
/**
 * The following schema fragment specifies the expected content contained within this class.
 * 
 * <complexType>
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element ref="{}property" maxOccurs="unbounded"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * 
 */
namespace PSModule.AlmLabMgmtClient.Result.Model
{
    [XmlRoot(ElementName = "properties")]
    public class Properties
    {
        [XmlElement(ElementName = "property")]
        public List<Property> ListOfProps { get; set; } = new List<Property>();
    }
}