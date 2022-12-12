using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;

namespace Managers.Argos
{
    public class ArgosImportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;

        private Dictionary<ArgosTag, ArenaObjectToXml> _parsers;

        public TextAsset baseXML;

        private void Start()
        {
            _parsers = GetComponents<ArenaObjectToXml>().ToDictionary((a) => a.Tag, a => a);
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.I) || !Input.GetKey(KeyCode.LeftControl))
                return;


            var doc = new XmlDocument();
            doc.LoadXml(baseXML.text);

            var arena = (XmlElement)doc.GetElementsByTagName("arena")[0];
            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];

            foreach (XmlElement element in arena.GetElementsByTagName("box"))
            {
                var newObject = _parsers[ArgosTag.Cube].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
            
            foreach (XmlElement element in arena.GetElementsByTagName("cylinder"))
            {
                var newObject = _parsers[ArgosTag.Cylinder].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
            
            foreach (XmlElement element in loopFunctions.GetElementsByTagName("circle"))
            {
                var newObject = _parsers[ArgosTag.Circle].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
            foreach (XmlElement element in loopFunctions.GetElementsByTagName("spawnCircle"))
            {
                var newObject = _parsers[ArgosTag.Circle].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
            foreach (XmlElement element in loopFunctions.GetElementsByTagName("plane"))
            {
                var newObject = _parsers[ArgosTag.Plane].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
        }
    }
}