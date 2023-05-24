using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using World.Arena;
using Random = System.Random;

namespace Managers.Argos.XML
{
    // This class provides methods to convert a polygon object in an Argos arena to and from XML elements
    public class ArgosPolygonLink : ArenaObjectToXml
    {

        // The Argos tag associated with this object type
        public override ArgosTag Tag => ArgosTag.Polygon;

        // Instantiates a new game object based on the given XML element
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);
            var controller = newObject.GetComponent<PolygonController>();

            // Set the polygon's border length, width, and count from the XML element
            controller.borderLength = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("length"));
            controller.borderWidth = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("width"));
            controller.bordersCount = int.Parse(element.GetAttribute("count"));
            
            

            // Set the polygon's position and rotation from the XML element
            ArgosHelper.SetPositionAndOrientationFromBody(element, newObject);

            // Add delta to put the polygon on the ground
            newObject.transform.Translate(new Vector3(0, 6f, 0));
            
            return newObject;
        }

        // Returns a list of XML elements that represent the given arena object
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var elements = new List<XmlElement>();

            // For each wall in the polygon, get its corresponding XML elements
            foreach (var child in arenaObject.transform.GetComponent<PolygonController>().walls)
                elements.AddRange(GetXMLWall(document, child, arenaObject.transform.localScale.x));

            var polygonXml = document.CreateElement(
                string.Empty,
                "polygon",
                string.Empty
            );

            var controller = arenaObject.GetComponent<PolygonController>();
            polygonXml.SetAttribute("id", GetInstanceID().ToString());
            polygonXml.SetAttribute("length", ArgosHelper.FloatToStringWithArgosFactor(controller.borderLength));
            polygonXml.SetAttribute("width",  ArgosHelper.FloatToStringWithArgosFactor(controller.borderWidth));
            polygonXml.SetAttribute("count", controller.bordersCount.ToString());
            ArgosHelper.InsertBodyTagFromTransform(document, polygonXml, arenaObject.transform);

            elements.Add(
                polygonXml
            );

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
        private IEnumerable<XmlElement> GetXMLWall(XmlDocument document, GameObject wall, float parentScale)
        {
            var node = document.CreateElement(string.Empty, "box", string.Empty);
            var localScale = wall.transform.localScale * parentScale; // x = z for arena object

            node.SetAttribute("id", wall.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));
            node.SetAttribute("movable", "false");
            node.SetAttribute("is_polygon", "true");

            ArgosHelper.InsertBodyTagFromTransform(document, node, wall.transform);

            return new List<XmlElement> { node };
        }
    }
}