using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Argos.XML
{
    // This abstract class provides methods to convert arena objects to and from XML elements
    public abstract class ArenaObjectToXml : MonoBehaviour
    {
        // The prefab that represents this object type in the scene
        public GameObject prefab;

        // The Argos tag associated with this object type
        public abstract ArgosTag Tag { get; }

        // Instantiates a new game object based on the given XML element
        public abstract GameObject InstantiateFromElement(XmlElement element);

        // Returns a list of XML elements that represent the given arena object
        public abstract List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject);

        // Returns the bounding box of the given arena object
        public abstract Bounds GetBounds(GameObject arenaObject);
    }
}
