using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Managers.Argos.XML;
using UnityEngine;
using Utils;

namespace Managers.Argos
{
    public class ArgosExportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;

        public TextAsset baseXML;

        private Dictionary<ArgosTag, ArenaObjectToXml> _parsers;

        private void Start()
        {
            _parsers = GetComponents<ArenaObjectToXml>().ToDictionary(a => a.Tag, a => a);
        }


        private void Update()
        {
            if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyDown(KeyCode.S))
                return;

            AsyncUpdate();
        }

        public void OnTriggerSave()
        {
            AsyncUpdate();
        }

        private async Task AsyncUpdate()
        {
            string outputPath;

            if (!GlobalVariables.HasKey(GlobalVariablesKey.ArgosFile) || Input.GetKey(KeyCode.LeftShift))
            {
                outputPath = await ArgosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser();
            }
            else
            {
                outputPath = GlobalVariables.Get<string>(GlobalVariablesKey.ArgosFile);
            }

            if (outputPath.Length > 0)
            {
                BuildXML(outputPath);
            }
        }

        private void BuildXML(string outputPath)
        {
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
                    if (argosTagForObject is ArgosTag.Circle or ArgosTag.Plane)
                        loopFunctions.AppendChild(xmlElement);
                    else
                        arena.AppendChild(xmlElement);

                boundsList.Add(parser.GetBounds(arenaGameObject));
            }

            // Compute arena bounds

            var summedBounds = boundsList.First();

            foreach (var bounds in boundsList.GetRange(1, boundsList.Count - 1)) summedBounds.Encapsulate(bounds);

            var distributePosition = (XmlElement)arena.SelectNodes("distribute/position")?[0];
            if (distributePosition != null)
            {
                var leftBottom = summedBounds.center - summedBounds.extents;
                var topRight = summedBounds.center + summedBounds.extents;


                distributePosition.SetAttribute("max", ArgosHelper.VectorToArgosVectorNoHeight(
                    new Vector3(Math.Min(leftBottom.x, topRight.x), 0, Math.Max(leftBottom.z, topRight.z))
                ));
                distributePosition.SetAttribute("min", ArgosHelper.VectorToArgosVectorNoHeight(
                    new Vector3(Math.Max(leftBottom.x, topRight.x), 0, Math.Min(leftBottom.z, topRight.z))
                ));
            }

            arena.SetAttribute("center",
                ArgosHelper.VectorToArgosVector(summedBounds.center)
            );

            var arenaSizeVector = new Vector3(
                Math.Max(1000, -summedBounds.extents.x * 2),
                Math.Max(1000, ArgosHelper.StringToFloatWithInverseArgosFactor("2")),
                Math.Max(1000, -summedBounds.extents.z * 2)
            );
            var vectorToArgosVector = ArgosHelper.VectorToArgosVector(arenaSizeVector);
            
            // Transform negative values to positive
            var split = vectorToArgosVector.Split(',');
            split[0] = Math.Abs(float.Parse(split[0])).ToString(CultureInfo.InvariantCulture);
            split[1] = Math.Abs(float.Parse(split[1])).ToString(CultureInfo.InvariantCulture);
            split[2] = Math.Abs(float.Parse(split[2])).ToString(CultureInfo.InvariantCulture);
            vectorToArgosVector = string.Join(",", split);
            
            arena.SetAttribute("size", vectorToArgosVector);

            ArgosFileLoader.SaveFile(outputPath, doc.OuterXml);
        }
    }
}