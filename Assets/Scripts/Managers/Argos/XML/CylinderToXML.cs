using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Argos.XML
{
    public class CylinderToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Cylinder;

        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);
            ArgosHelper.SetPositionAndOrientationFromBody(element, newObject);
            var radius = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("radius"));
            var height = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("height"));
            newObject.transform.localScale = new Vector3(radius, height, radius);

            return newObject;
        }

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "cylinder", string.Empty);
            var localScale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(localScale.y));
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x / 2));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, arenaObject.transform);

            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}