// Import required namespaces

using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

// Define the namespace and class name
namespace Managers.Argos.XML
{
    public class ArgosCircleLink : ArenaObjectToXml
    {
// Override the Tag property of the base class to specify the tag name for circles
        public override ArgosTag Tag => ArgosTag.Circle;

        // Instantiate a game object from an XML element and return it
        public override GameObject InstantiateFromElement(XmlElement element)
        {
            // Instantiate a new game object from a prefab
            var newObject = Instantiate(prefab);

            // Get the radius of the circle from the "radius" attribute of the XML element and set the scale of the game object
            var radius = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("radius"));
            newObject.transform.localScale = new Vector3(radius * 2, 1, radius * 2);

            // Get the position of the circle from the "position" attribute of the XML element and set the position of the game object
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("position"));
            position += new Vector3(0, 1, 0);
            newObject.transform.position = position;

            // Set the color of the game object from the "color" attribute of the XML element
            ColorUtility.TryParseHtmlString(element.GetAttribute("color"), out var color);
            newObject.GetComponent<Renderer>().material.color = color;

            // Get the SpawnCircleEditableItem component of the game object and update its values
            var spawnCircleEditableItem = newObject.GetComponent<SpawnCircleEditableItem>();
            spawnCircleEditableItem.UpdateValues(new Dictionary<string, string>
            {
                { "spawn_circle", (element.Name == "spawnCircle" ? 1 : 0).ToString() }
            });

            // Return the game object
            return newObject;
        }

        // Generate a list of XML elements from a game object and return it
        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            // Determine if the game object is an editable circle or a regular circle
            var isEditableCircle = false;
            var spawnCircleEditableItem = arenaObject.GetComponent<SpawnCircleEditableItem>();
            if (spawnCircleEditableItem) isEditableCircle = spawnCircleEditableItem.isSpawnCircle;

            // Create a new XML element for the circle
            var node = document.CreateElement(string.Empty, isEditableCircle ? "spawnCircle" : "circle", string.Empty);

            // Set the "id" attribute of the XML element to the instance ID of the game object
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());

            // Set the "radius" attribute of the XML element to half of the x scale of the game object
            var localScale = arenaObject.transform.localScale;
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x / 2));

            // Set the "position" attribute of the XML element to the position of the game object
            node.SetAttribute("position", ArgosHelper.VectorToArgosVectorNoHeight(arenaObject.transform.position));

            // Set the "spawn_circle" attribute of the XML element to "true" if the game object is an editable circle, and "false" otherwise
            node.SetAttribute("spawn_circle", isEditableCircle ? "true" : "false");

            // If the game object has a ColorEditableItem component, set the "color" attribute of the XML element to the color of the component
            var colorEditableItem = arenaObject.GetComponent<ColorEditableItem>();


            if (colorEditableItem)
            {
                Debug.Log("Color editable item");
                var color = colorEditableItem.GetColor();
                Debug.Log(color);
                node.SetAttribute("color", color == "FFFFFFFF" ? "white" : "black");
            }

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