// Import required namespaces

using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

// Define the namespace and class name
namespace Managers.Argos.XML
{
    public class ArgosLightLink : ArenaObjectToXml
    {
        // Return the Argos tag for this link
        public override ArgosTag Tag => ArgosTag.Light;

        // Create a game object from an XML element
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);
            var lightEditableItem = newObject.GetComponent<LightEditableItem>();

            // Set the light intensity from the XML element
            lightEditableItem.intensity = ArgosHelper.StringToFloat(element.GetAttribute("intensity"));

            // Set the game object's position and rotation from the XML element
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("position"));
            newObject.transform.position = position;
            newObject.transform.rotation = ArgosHelper.ArgosVectorToQuaternion(element.GetAttribute("orientation"));

            return newObject;
        }

        // Create a list of XML elements from a game object
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var lightEditableItem = arenaObject.GetComponent<LightEditableItem>().intensity;
            var node = document.CreateElement(string.Empty, "light", string.Empty);

            // Set the attributes of the XML element from the game object
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("intensity", ArgosHelper.FloatToString(lightEditableItem));
            node.SetAttribute("color", "yellow");
            node.SetAttribute("medium", "leds");
            node.SetAttribute("orientation", ArgosHelper.QuaternionToArgosVector(arenaObject.transform.rotation));

            // Set the position of the XML element from the game object
            var transformPosition = arenaObject.transform.position;
            transformPosition.y = 10f;
            node.SetAttribute("position", ArgosHelper.VectorToArgosVector(transformPosition));

            return new List<XmlElement> { node };
        }

        // Get the bounds of a game object
        public override Bounds GetBounds(GameObject arenaObject)
        {
            return new Bounds();
        }
    }
}