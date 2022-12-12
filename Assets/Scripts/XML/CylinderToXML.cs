using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

namespace XML
{
    public class CylinderToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Cylinder;

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "cylinder", string.Empty);
            var localScale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(localScale.y));
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x/2)); 
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