using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Export.XML
{
    public class PlaneToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Plane;

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "rectangle", string.Empty);
            var scale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("width", ArgosHelper.FloatToStringWithArgosFactor(scale.x));
            node.SetAttribute("height", ArgosHelper.FloatToStringWithArgosFactor(scale.z));
            node.SetAttribute("color", "black");
            node.SetAttribute("center", ArgosHelper.VectorToArgosVectorNoHeight(arenaObject.transform.position));
            node.SetAttribute("angle", ArgosHelper.FloatToString(arenaObject.transform.eulerAngles.y));
            
            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}