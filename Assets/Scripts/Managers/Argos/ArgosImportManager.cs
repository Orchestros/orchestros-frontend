using System.Collections.Generic;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;
using Utils;

namespace Managers.Argos
{
    // This class is responsible for importing the map data from an Argos compatible XML file
    public class ArgosImportManager : MonoBehaviour
    {
        // Reference to the ArenaObjectsManager which manages the objects in the scene
        public ArenaObjectsManager arenaObjectsManager;

        public ArgosFileLoader argosFileLoader;
        
        // Base XML file used as a template for the imported file
        public TextAsset baseXML;

        // Dictionary containing parsers for each object in the scene
        private readonly Dictionary<ArgosTag, ArenaObjectToXml> _parsers = new();

        private void Start()
        {
            // Get all the ArenaObjectToXml components and create a dictionary using their tags as keys
            foreach (var arenaObjectToXml in GetComponents<ArenaObjectToXml>())
            {
                _parsers[arenaObjectToXml.Tag] = arenaObjectToXml;
            }

            // If the Argos file path is set, import the file
            if (GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile) &&
                !string.IsNullOrEmpty(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile)))
            {
                ImportArgosFile(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile));
            }
        }

        private void Update()
        {
            // Check if Left Control key and I key are pressed simultaneously
            if (!Input.GetKeyDown(KeyCode.I) || !Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }

            AsyncUpdate();
        }

        // This method is called asynchronously when the I key and Left Control key are pressed simultaneously
        private async void AsyncUpdate()
        {
            var file = argosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser();
            ImportArgosFile(file);
        }

        // This method imports the XML file and creates objects in the scene based on the XML elements
        private void ImportArgosFile(string filePath)
        {
            var doc = new XmlDocument();
            var fileContent = argosFileLoader.GetArgosFileLoader().GetContentFromPathOrUrl(filePath);

            doc.LoadXml(fileContent);

            var arena = (XmlElement)doc.GetElementsByTagName("arena")[0];
            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];

            // Create objects in the scene for all "box" XML elements in the "arena" section
            foreach (XmlElement element in arena.GetElementsByTagName("box"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Cube);
            }

            // Create objects in the scene for all "cylinder" XML elements in the "arena" section
            foreach (XmlElement element in arena.GetElementsByTagName("cylinder"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Cylinder);
            }

            // Create objects in the scene for all "light" XML elements in the "arena" section
            foreach (XmlElement element in arena.GetElementsByTagName("light"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Light);
            }

            // Create objects in the scene for all "circle" and "spawnCircle" XML elements in the "loop_functions" section
            foreach (XmlElement element in loopFunctions.GetElementsByTagName("circle"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Circle);
            }

            foreach (XmlElement element in loopFunctions.GetElementsByTagName("spawnCircle"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Circle);
            }

            // Create objects in the scene for all "rectangle" XML elements in the "loop_functions" section
            foreach (XmlElement element in loopFunctions.GetElementsByTagName("rectangle"))
            {
                InstantiateObjectFromElement(element, ArgosTag.Plane);
            }
        }

        // This method instantiates a new object in the scene based on the XML element and the object type tag
        private void InstantiateObjectFromElement(XmlElement element, ArgosTag objectTypeTag)
        {
            // If the dictionary does not contain the parser for the object type tag, return
            if (!_parsers.ContainsKey(objectTypeTag))
            {
                return;
            }

            // Instantiate the object using the appropriate parser
            var newObject = _parsers[objectTypeTag].InstantiateFromElement(element);

            // Add the object to the ArenaObjectsManager
            arenaObjectsManager.OnObjectAdded(newObject);
        }
    }
}