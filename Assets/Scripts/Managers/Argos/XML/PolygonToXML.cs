using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using World.Arena;

namespace Managers.Argos.XML
{
    // This class provides methods to convert a polygon object in an Argos arena to and from XML elements
    public class PolygonToXML : ArenaObjectToXml
    {
        // The Argos tag associated with this object type
        public override ArgosTag Tag => ArgosTag.Polygon;

        // Instantiates a new game object based on the given XML element
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            throw new NotImplementedException();
        }

        // Returns a list of XML elements that represent the given arena object
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var elements = new List<XmlElement>();

            // For each wall in the polygon, get its corresponding XML elements
            foreach (var child in arenaObject.transform.GetComponent<PolygonController>().walls)
                elements.AddRange(GetXMLWall(document, child, arenaObject.transform.localScale.x));

            return elements;
        }

        // Returns the bounding box of the given arena object
        public override Bounds GetBounds(GameObject arenaObject)
        {
            var wallObjects = arenaObject.GetComponent<PolygonController>().walls;

            var newBounds = wallObjects.First().GetComponent<Renderer>().bounds;

            foreach (var wallObject in wallObjects.GetRange(1, wallObjects.Count - 1))
                newBounds.Encapsulate(wallObject.GetComponent<Renderer>().bounds);

            return newBounds;
        }

        // Returns a list of XML elements that represent the given wall object in the polygon
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