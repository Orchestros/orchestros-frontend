using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
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
                elements.AddRange(GetXMLWall(document, child));
            }

            return elements;
        }
        
        private List<XmlElement> GetXMLWall(XmlDocument document, GameObject wall)
        {
            var node = document.CreateElement(string.Empty, "box", string.Empty);
            var localScale = wall.transform.localScale*transform.localScale.x; // x = z for arena object

            node.SetAttribute("id", wall.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, wall.transform);
            
            return new List<XmlElement> { node };
        }
    }
}