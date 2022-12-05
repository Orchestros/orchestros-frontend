using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

namespace XML
{
    public class CubeToXML : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var x = document.CreateElement("box");
            var localScale = transform.localScale;

            x.SetAttribute("id", gameObject.GetInstanceID().ToString());
            x.SetAttribute("size", VectorToArgosVector(localScale));
            x.SetAttribute("movable", "false");

            var y = document.CreateElement("body");
            y.SetAttribute("position", VectorToArgosVector(transform.position));
            y.SetAttribute("orientation", VectorToArgosVector(transform.rotation.eulerAngles));
            x.AppendChild(y);
            
            return new List<XmlElement>{x};
        }

        private static string VectorToArgosVector(Vector3 vector)
        {
            return vector.x.ToString(CultureInfo.InvariantCulture) + "," +
                   vector.z.ToString(CultureInfo.InvariantCulture) + "," +
                   0;
        }
    }
}