// Import required namespaces

using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Define the namespace and class name
namespace Managers.Argos.XML
{
    public class CylinderToXML : ArenaObjectToXml
    {
        // Return the Argos tag for this link
        public override ArgosTag Tag => ArgosTag.Cylinder;

        // Create a game object from an XML element
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);

            // Set the game object's position and rotation from the XML element
            ArgosHelper.SetPositionAndOrientationFromBody(element, newObject);

            // Set the game object's scale from the XML element
            var radius = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("radius"));
            var height = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("height"));
            newObject.transform.localScale = new Vector3(radius, height, radius);

            return newObject;
        }

        // Create a list of XML elements from a game object
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "cylinder", string.Empty);
            var localScale = arenaObject.transform.localScale;

            // Set the attributes of the XML element from the game object
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(localScale.y));
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x / 2));
            node.SetAttribute("movable", "false");

            // Set the body tag of the XML element from the game object
            ArgosHelper.InsertBodyTagFromTransform(document, node, arenaObject.transform);

            return new List<XmlElement> { node };
        }

        // Get the bounds of a game object
        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}