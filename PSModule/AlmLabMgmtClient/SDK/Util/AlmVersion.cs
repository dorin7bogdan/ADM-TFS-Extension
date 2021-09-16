
using System.Xml.Serialization;

[XmlRoot(ElementName = "SiteVersions")]
public class AlmVersion
{
    [XmlElement(ElementName = "MajorVersion")]
    public string MajorVersion { get; set; }

    [XmlElement(ElementName = "MinorVersion")]
    public string MinorVersion { get; set; }
}
