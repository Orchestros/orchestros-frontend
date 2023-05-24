using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;
using Utils;

namespace Managers.Argos
{
    // This class is responsible for exporting the map data as an Argos compatible XML file
    public class ArgosExportManager : MonoBehaviour
    {
        // Reference to the ArenaObjectsManager which manages the objects in the scene
        public ArenaObjectsManager arenaObjectsManager;
        public ArgosFileLoader argosFileLoader;

        // Base XML file used as a template for the exported file
        public TextAsset baseXML;

        // Dictionary containing parsers for each object in the scene
        private Dictionary<ArgosTag, ArenaObjectToXml> _parsers;

        private void Start()
        {
            // Get all the ArenaObjectToXml components and create a dictionary using their tags as keys
            _parsers = GetComponents<ArenaObjectToXml>().ToDictionary(a => a.Tag, a => a);
        }

        private void Update()
        {
            // Check if Left Control key and S key are pressed simultaneously
            if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyDown(KeyCode.S))
            {
                return;
            }

            AsyncUpdate();
        }

        // This method is called when the save trigger is activated
        public void OnTriggerSave()
        {
            AsyncUpdate();
        }

        // This method is called asynchronously when the save trigger is activated or when Left Control and S keys are pressed simultaneously
        private void AsyncUpdate()
        {
            // If the Argos file path is not set or Left Shift key is pressed, show the file dialog to get the file path from the user
            var outputPath = GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile);
            if (!GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile) ||
                string.IsNullOrEmpty(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile)) ||
                Input.GetKey(KeyCode.LeftShift))
            {
                argosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser(file =>
                {
                    BuildXML(file);
                    GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
                }, newFile: true);
            }
            else
            {
                if (outputPath.Length > 0)
                {
                    BuildXML(outputPath);
                }
            }
        }

        // Function to remove elements with a specific tag name from the XML document
        private void RemoveElements(XmlNode node, string tagName)
        {
            var elementsToRemove = new List<XmlNode>();

            // Find and store elements with the specified tag name
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == tagName)
                {
                    elementsToRemove.Add(childNode);
                }
                else
                {
                    RemoveElements(childNode, tagName);
                }
            }

            // Remove the stored elements from the parent node
            foreach (var element in elementsToRemove)
            {
                node.RemoveChild(element);
            }
        }

        // The base xml to which the objects will be added.
        // If in edit mode, the base xml will be the one from the file.
        // If in create mode, the base xml will be the one from the .orchesta/template.argos file.
        // Otherwise, the base xml will be the one from the baseXML.text (a Unity TextAsset).
        private string GetBaseXml()
        {
            // if argos file is empty in global variables, return the base xml
            if (!GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile) ||
                string.IsNullOrEmpty(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile)))
            {
                # if !UNITY_WEBGL
                
                // Check if .orchesta/template.argos file exists at user's home directory
                var templateFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Path.Combine(".orchestra", "template.argos"));
                Debug.Log(templateFilePath);
                if (File.Exists(templateFilePath))
                {
                    // If the file exists, return the xml from the file
                    return File.ReadAllText(templateFilePath);
                }
                # endif
                return baseXML.text;
            }
            

            // if argos file is not empty, return the xml from the file
            var fileContent = argosFileLoader.GetArgosFileLoader()
                .GetContentFromPathOrUrl(GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile));

            // Open in XML and remove all the polygon box cylinder light circle spawnCircle rectangle
            // no matter the depth of the element.
            var doc = new XmlDocument();
            doc.LoadXml(fileContent);
            var root = doc.DocumentElement;
            // Remove specific elements from the XML document
            RemoveElements(root, "polygon");
            RemoveElements(root, "box");
            RemoveElements(root, "cylinder");
            RemoveElements(root, "light");
            RemoveElements(root, "circle");
            RemoveElements(root, "spawnCircle");
            RemoveElements(root, "rectangle");

            return root!.OuterXml;
        }

        // This method builds the XML file and saves it to the specified path
        private void BuildXML(string outputPath)
        {
            var doc = new XmlDocument();
            doc.LoadXml(GetBaseXml());

            var arena = (XmlElement)doc.GetElementsByTagName("arena")[0];
            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];

            // Set the arena center to (0,0,0)
            arena.SetAttribute("center", "0,0,0");

            // List to store the bounds of all objects in the scene
            var boundsList = new List<Bounds>();

            // Iterate through all the objects in the scene
            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var argosTagForObject = arenaGameObject.GetComponent<ArgosInfo>().tag;

                // If the object does not have a parser associated with its tag, skip it
                if (!_parsers.ContainsKey(argosTagForObject))
                {
                    continue;
                }

                var parser = _parsers[argosTagForObject];


                // Get the XML elements for the object using its parser
                var xmlElements = parser.GetXMLElements(doc, arenaGameObject);
                foreach (var xmlElement in xmlElements)
                {
                    // Append the XML element to the appropriate parent
                    if (argosTagForObject is ArgosTag.Circle or ArgosTag.Plane)
                    {
                        loopFunctions.AppendChild(xmlElement);
                    }
                    else
                    {
                        if (argosTagForObject is ArgosTag.Polygon)
                        {
                            // if is last element, add to loop functions
                            if (xmlElements.IndexOf(xmlElement) == xmlElements.Count - 1)
                            {
                                loopFunctions.AppendChild(xmlElement);
                                continue;
                            }
                        }

                        arena.AppendChild(xmlElement);
                    }
                }

                // Add the bounds of the object to the bounds list
                boundsList.Add(parser.GetBounds(arenaGameObject));
            }

            // Compute the arena bounds by encapsulating all object bounds
            var summedBounds = boundsList.First();

            foreach (var bounds in boundsList.GetRange(1, boundsList.Count - 1))
            {
                summedBounds.Encapsulate(bounds);
            }

            // Get the "distribute/position" element and set its "max" and "min" attributes based on the arena bounds
            var distributePosition = (XmlElement)arena.SelectNodes("distribute/position")?[0];

            if (distributePosition != null)
            {
                var leftBottom = summedBounds.center - summedBounds.extents;
                var topRight = summedBounds.center + summedBounds.extents;

                distributePosition.SetAttribute("max", ArgosHelper.VectorToArgosVectorNoHeight(
                    new Vector3(Math.Min(leftBottom.x, topRight.x), 0, Math.Max(leftBottom.z, topRight.z))));

                distributePosition.SetAttribute("min", ArgosHelper.VectorToArgosVectorNoHeight(
                    new Vector3(Math.Max(leftBottom.x, topRight.x), 0, Math.Min(leftBottom.z, topRight.z))));
            }

            // Set the arena center and size attributes based on the arena bounds
            arena.SetAttribute("center", ArgosHelper.VectorToArgosVector(summedBounds.center));

            var arenaSizeVector = new Vector3(
                Math.Max(1000, -summedBounds.extents.x * 2),
                Math.Max(1000, ArgosHelper.StringToFloatWithInverseArgosFactor("2")),
                Math.Max(1000, -summedBounds.extents.z * 2));

            var vectorToArgosVector = ArgosHelper.VectorToArgosVector(arenaSizeVector);

            // Transform negative values to positive
            var split = vectorToArgosVector.Split(',');
            split[0] = Math.Abs(float.Parse(split[0])).ToString(CultureInfo.InvariantCulture);
            split[1] = Math.Abs(float.Parse(split[1])).ToString(CultureInfo.InvariantCulture);
            split[2] = Math.Abs(float.Parse(split[2])).ToString(CultureInfo.InvariantCulture);
            vectorToArgosVector = string.Join(",", split);

            arena.SetAttribute("size", vectorToArgosVector);

            // Save the XML file to the specified path
            argosFileLoader.SaveXmlFile(outputPath, doc);
        }
    }
}