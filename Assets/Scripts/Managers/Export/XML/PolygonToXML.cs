using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using World.Arena;

namespace Managers.Export.XML
{
    public class PolygonToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Polygon;

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var elements = new List<XmlElement>();

            foreach (var child in arenaObject.transform.GetComponent<PolygonController>().Walls)
            {
                elements.AddRange(GetXMLWall(document, child, arenaObject.transform.localScale.x));
            }

            return elements;
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            var wallObjects = arenaObject.GetComponent<PolygonController>().Walls;

            var newBounds = wallObjects.First().GetComponent<Renderer>().bounds;

            foreach (var wallObject in wallObjects.GetRange(1,wallObjects.Count-1))
            {
                newBounds.Encapsulate(wallObject.GetComponent<Renderer>().bounds);
            }

            return newBounds;
        }

        private static IEnumerable<XmlElement> GetXMLWall(XmlDocument document, GameObject wall, float parentScale)
        {
            var node = document.CreateElement(string.Empty, "box", string.Empty);
            var localScale = wall.transform.localScale * parentScale; // x = z for arena object

            node.SetAttribute("id", wall.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, wall.transform);

            return new List<XmlElement> { node };
        }
    }
}