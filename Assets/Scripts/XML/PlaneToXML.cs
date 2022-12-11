using System.Collections.Generic;
using System.Xml;

namespace XML
{
    public class PlaneToXML : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var node = document.CreateElement(string.Empty, "surface", string.Empty);
            var localScale = transform.localScale;

            node.SetAttribute("id", gameObject.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVectorNoHeight(localScale));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, transform);

            return new List<XmlElement> { node };
        }

    
    }
}