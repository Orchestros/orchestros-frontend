// Import required namespaces

using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Define the namespace and class name
namespace Managers.Argos.XML
{
    public class ArgosCubeLink : ArenaObjectToXml
    {
// Override the Tag property of the base class to specify the tag name for cubes
        public override ArgosTag Tag => ArgosTag.Cube;

        // Instantiate a game object from an XML element and return it
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            // Instantiate a new game object from a prefab
            var newObject = Instantiate(prefab);

            // Set the position and orientation of the game object from the "body" tag of the XML element
            ArgosHelper.SetPositionAndOrientationFromBody(element, newObject);

            // Set the scale of the game object from the "size" attribute of the XML element
            newObject.transform.localScale = ArgosHelper.ArgosVectorToVectorAbsolute(element.GetAttribute("size"));

            // Return the game object
            return newObject;
        }

        // Generate a list of XML elements from a game object and return it
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            // Create a new XML element for the cube
            var node = document.CreateElement(string.Empty, "box", string.Empty);

            // Set the "id" attribute of the XML element to the instance ID of the game object
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());

            // Set the "size" attribute of the XML element to the scale of the game object
            var localScale = arenaObject.transform.localScale;
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));

            // Set the "movable" attribute of the XML element to "false"
            node.SetAttribute("movable", "false");

            // Insert a "body" tag in the XML element containing the position and orientation of the game object
            ArgosHelper.InsertBodyTagFromTransform(document, node, arenaObject.transform);

            // Return a list containing the XML element
            return new List<XmlElement> { node };
        }

        // Get the bounds of a game object and return them
        public override Bounds GetBounds(GameObject arenaObject)
        {
            // Return the bounds of the game object's renderer component
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}