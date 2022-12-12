using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;

namespace Managers.Argos
{
    public class ArgosExportManager : MonoBehaviour
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
            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))
                return;


            var doc = new XmlDocument();
            doc.LoadXml(baseXML.text);


            var arena = (XmlElement)doc.GetElementsByTagName("arena")[0];
            var loopFunctions = (XmlElement)doc.GetElementsByTagName("loop_functions")[0];
            arena.SetAttribute("center", "0,0,0");

            var boundsList = new List<Bounds>();

            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var argosTagForObject = arenaGameObject.GetComponent<ArgosInfo>().tag;

                if (!_parsers.ContainsKey(argosTagForObject)) continue;

                var parser = _parsers[argosTagForObject];
                foreach (var xmlElement in parser.GetXMLElements(doc, arenaGameObject))
                {
                    if (argosTagForObject is ArgosTag.Circle or ArgosTag.Plane)
                    {
                        loopFunctions.AppendChild(xmlElement);
                    }
                    else
                    {
                        arena.AppendChild(xmlElement);
                    }
                }

                boundsList.Add(parser.GetBounds(arenaGameObject));
            }

            // Compute arena bounds

            var summedBounds = boundsList.First();

            foreach (var bounds in boundsList.GetRange(1, boundsList.Count - 1))
            {
                summedBounds.Encapsulate(bounds);
            }

            var distributePosition = (XmlElement) arena.SelectNodes("distribute/position")?[0];
            if (distributePosition != null)
            {
                distributePosition.SetAttribute("max", ArgosHelper.VectorToArgosVectorNoHeight(
                    summedBounds.center - new Vector3(10, 0, -10)
                )); 
                distributePosition.SetAttribute("min", ArgosHelper.VectorToArgosVectorNoHeight(
                    summedBounds.center + new Vector3(10, 0, -10)
                ));
            }

            arena.SetAttribute("center",
                ArgosHelper.VectorToArgosVector(summedBounds.center));
            arena.SetAttribute("size", ArgosHelper.VectorToArgosVector(new Vector3(
                -summedBounds.extents.x * 2,
                summedBounds.extents.y * 2,
                -summedBounds.extents.z * 2
            )));

            Debug.Log(doc.OuterXml);
        }
    }
}