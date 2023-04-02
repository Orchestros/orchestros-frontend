using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

namespace Managers.Argos.XML
{
    public class PlaneToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Plane;

        public override GameObject InstantiateFromElement(XmlElement element)
        {
            // Instantiate a new object from the prefab
            var newObject = Instantiate(prefab);

            // Set the object's scale based on the "width" and "height" attributes
            var width = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("width"));
            var height = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("height"));
            newObject.transform.localScale = new Vector3(width, 1, height);

            // Set the object's color based on the "color" attribute
            ColorUtility.TryParseHtmlString(element.GetAttribute("color"), out var color);
            newObject.GetComponent<Renderer>().material.color = color;

            // Set the object's position and rotation based on the "center" and "rotation" attributes
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("center"));
            position += new Vector3(0, 1, 0); // Raise the object by 1 unit to match the arena floor
            newObject.transform.position = position;
            newObject.transform.rotation =
                Quaternion.Euler(0, ArgosHelper.StringToFloat(element.GetAttribute("rotation")), 0);

            return newObject;
        }

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            // Create a new "rectangle" element for the object
            var node = document.CreateElement(string.Empty, "rectangle", string.Empty);
            var scale = arenaObject.transform.localScale;
            var colorEditableItem = arenaObject.GetComponent<ColorEditableItem>();

            // Set the attributes of the "rectangle" element based on the object's properties
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("width", ArgosHelper.FloatToStringWithArgosFactor(scale.x));
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(scale.z));
            node.SetAttribute("center", ArgosHelper.VectorToArgosVectorNoHeight(arenaObject.transform.position));
            node.SetAttribute("angle", ArgosHelper.FloatToString(arenaObject.transform.eulerAngles.y));

            // Get the color of the object and set the "color" attribute
            if (!colorEditableItem) return new List<XmlElement> { node };

            var color = colorEditableItem.GetColor();
            node.SetAttribute("color",
                color is "FFFFFFFF"
                    ? "white"
                    : "black"); // Set the color to "white" if it's "ffffff", otherwise set it to "black"

            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            // Get the bounds of the object's renderer
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}