using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using World.Arena;

namespace XML
{
    public class PolygonToXML : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var elements = new List<XmlElement>();

            foreach (var child in transform.GetComponent<PolygonController>().Walls)
            {
                elements.AddRange(child.GetOrAddComponent<CubeToXML>().GetXMLElements(document));
            }

            return elements;
        }
        
    }
}