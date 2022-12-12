using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Argos.XML
{
    public abstract class ArenaObjectToXml : MonoBehaviour
    {
        public GameObject prefab;

        public abstract ArgosTag Tag { get; }


        public abstract GameObject InstantiateFromElement(XmlElement element);
        
        public abstract List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject);

        public abstract Bounds GetBounds(GameObject arenaObject);
    }
}