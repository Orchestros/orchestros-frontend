using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Export.XML
{
    public abstract class ArenaObjectToXml : MonoBehaviour
    {
        public abstract ArgosTag Tag { get; }
        public abstract List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject);

        public abstract Bounds GetBounds(GameObject arenaObject);
    }
}