using System.Collections.Generic;
using System.Numerics;
using System.Xml;

namespace XML
{
    public class CubeToXML : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var node = document.CreateElement(string.Empty, "box", string.Empty);
            var localScale = transform.lossyScale;

            node.SetAttribute("id", gameObject.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, transform);
            
            return new List<XmlElement> { node };
        }

    
    }
}