using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace XML
{
    public abstract class ArenaObjectToXml : MonoBehaviour
    {
        public abstract List<XmlElement> GetXMLElements(XmlDocument document);
    }
}