using System.Xml;
using Managers.Argos.XML;
using UnityEngine;

namespace Managers
{
    public class DemoExportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;

        public TextAsset baseXML;


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))
                return;


            var doc = new XmlDocument();
            doc.LoadXml(baseXML.text);


            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];

            var demo = doc.CreateElement("demo");
            demo.SetAttribute("id", "demo-1");

            var currentIndex = 0;

            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var argosTagForObject = arenaGameObject.GetComponent<ArgosInfo>().tag;

                if (argosTagForObject != ArgosTag.Robot) continue;

                var robot = doc.CreateElement("epuck");
                robot.SetAttribute("id", "Epuck-" + currentIndex);
                robot.SetAttribute("position",
                    ArgosHelper.VectorToArgosVectorNoHeight2D(arenaGameObject.transform.position));
                demo.AppendChild(robot);
                currentIndex += 1;
            }

            loopFunctions.AppendChild(demo);
            Debug.Log(doc.OuterXml);
        }
    }
}