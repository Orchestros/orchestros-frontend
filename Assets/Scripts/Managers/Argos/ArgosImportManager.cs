using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;
using UnityEngine.Windows;
using Utils;
using Input = UnityEngine.Input;

namespace Managers.Argos
{
    public class ArgosImportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;

        public TextAsset baseXML;

        private Dictionary<ArgosTag, ArenaObjectToXml> _parsers;

        private void Start()
        {
            _parsers = GetComponents<ArenaObjectToXml>().ToDictionary(a => a.Tag, a => a);
            
            
            if (GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile))
            {
                ImportArgosFile(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile));
            }
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.I) || !Input.GetKey(KeyCode.LeftControl))
                return;
            
            var file = ArgosFileLoader.GetArgosFilePathFromUser();
            ImportArgosFile(file);
        }

        private void ImportArgosFile(string filePath)
        {
            
            var doc = new XmlDocument();
            var fileContent = System.IO.File.ReadAllText(filePath);
            doc.LoadXml(fileContent);

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

            foreach (XmlElement element in arena.GetElementsByTagName("light"))
            {
                var newObject = _parsers[ArgosTag.Light].InstantiateFromElement(element);
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

            foreach (XmlElement element in loopFunctions.GetElementsByTagName("rectangle"))
            {
                var newObject = _parsers[ArgosTag.Plane].InstantiateFromElement(element);
                arenaObjectsManager.OnObjectAdded(newObject);
            }
        }
    }
}