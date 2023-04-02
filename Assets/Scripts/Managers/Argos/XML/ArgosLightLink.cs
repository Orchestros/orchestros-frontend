using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

namespace Managers.Argos.XML
{
    public class ArgosLightLink : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Light;

        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);
            var lightEditableItem = newObject.GetComponent<LightEditableItem>();
            lightEditableItem.intensity = ArgosHelper.StringToFloat(element.GetAttribute("intensity"));
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("position"));
            newObject.transform.position = position;
            newObject.transform.rotation = ArgosHelper.ArgosVectorToQuaternion(element.GetAttribute("orientation"));

            return newObject;
        }

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var lightEditableItem = arenaObject.GetComponent<LightEditableItem>().intensity;
            var node = document.CreateElement(string.Empty, "light", string.Empty);

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("intensity", ArgosHelper.FloatToString(lightEditableItem));
            node.SetAttribute("color", "yellow");
            node.SetAttribute("medium", "leds");
            node.SetAttribute("orientation", ArgosHelper.QuaternionToArgosVector(arenaObject.transform.rotation));
            var transformPosition = arenaObject.transform.position;
            transformPosition.y = 10f;
            node.SetAttribute("position", ArgosHelper.VectorToArgosVector(transformPosition));

            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return new Bounds();
        }
    }
}