using System;
using System.Text.RegularExpressions;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;
using Utils;

namespace Managers
{
    public class DemoExportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;

        public string currentXML;


        private void Start()
        {
       
            if (GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile))
            {
                currentXML = GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile);
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))
                return;
            
            var doc = new XmlDocument();
            doc.LoadXml(System.IO.File.ReadAllText(currentXML));
            
            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];

            // Get last demo id
            var greatestDemoId = 0;
            
            foreach (var oldDemo in loopFunctions.GetElementsByTagName("demo"))
            {
                var idString = ((XmlElement)oldDemo).GetAttribute("id");
                // remove any alpha characters
                idString = Regex.Replace(idString, "[^0-9]", "");

                if (!int.TryParse(idString, out var demoId)) continue;
                
                if (demoId > greatestDemoId)
                {
                    greatestDemoId = demoId;
                }
            }
            
            var demo = doc.CreateElement("demo");
            demo.SetAttribute("id", "demo_" + (greatestDemoId + 1));

            var currentIndex = 0;

            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var argosTagForObject = arenaGameObject.GetComponent<ArgosInfo>().tag;

                if (argosTagForObject != ArgosTag.Robot) continue;

                print(argosTagForObject);
                
                var robot = doc.CreateElement("epuck");
                robot.SetAttribute("id", "Epuck-" + currentIndex);
                robot.SetAttribute("position",
                    ArgosHelper.VectorToArgosVectorNoHeight2D(arenaGameObject.transform.position));
                demo.AppendChild(robot);
                currentIndex += 1;
            }

            loopFunctions.AppendChild(demo);
            
            doc.Save(currentXML);
        }
    }
}