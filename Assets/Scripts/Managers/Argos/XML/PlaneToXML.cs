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
            var newObject = Instantiate(prefab);
            var width = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("width"));
            var height = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("height"));
            newObject.transform.localScale = new Vector3(width, 1, height);
            
            ColorUtility.TryParseHtmlString(element.GetAttribute("color"), out var color);
            newObject.GetComponent<Renderer>().material.color = color;
            
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("center"));
            position += new Vector3(0, 1, 0);
            newObject.transform.position = position;
            newObject.transform.rotation =
                Quaternion.Euler(0, ArgosHelper.StringToFloat(element.GetAttribute("rotation")), 0);

            return newObject;
        }

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "rectangle", string.Empty);
            var scale = arenaObject.transform.localScale;
            var colorEditableItem = arenaObject.GetComponent<ColorEditableItem>();
            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("width", ArgosHelper.FloatToStringWithArgosFactor(scale.x));
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(scale.z));
            node.SetAttribute("center", ArgosHelper.VectorToArgosVectorNoHeight(arenaObject.transform.position));
            node.SetAttribute("angle", ArgosHelper.FloatToString(arenaObject.transform.eulerAngles.y));

            
            // Get color
            if (colorEditableItem)
            {
                var color = colorEditableItem.GetColor();
                node.SetAttribute("color", color is "ffffff" ? "white" : "black");
            }
            
            return new List<XmlElement> { node };
            
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}