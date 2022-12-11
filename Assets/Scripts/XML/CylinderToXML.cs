using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace XML
{
    public class Cylinder : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var node = document.CreateElement(string.Empty, "cylinder", string.Empty);
            var localScale = transform.localScale;

            node.SetAttribute("id", gameObject.GetInstanceID().ToString());
            node.SetAttribute("height", localScale.y.ToString(CultureInfo.InvariantCulture));
            node.SetAttribute("radius", (localScale.x/2).ToString(CultureInfo.InvariantCulture)); 
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, transform);

            return new List<XmlElement> { node };
        }
    }
}