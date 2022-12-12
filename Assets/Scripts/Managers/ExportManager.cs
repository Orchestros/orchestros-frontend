using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using XML;

namespace Managers
{
    public class ExportManager : MonoBehaviour
    {
        public float x = 0;
        public ArenaObjectsManager arenaObjectsManager;

        private Dictionary<ArgosTag, ArenaObjectToXml> _parsers;

        private void Start()
        {
            _parsers = GetComponents<ArenaObjectToXml>().ToDictionary((a) => a.Tag, a => a);
        }


        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))
                return;


            var doc = new XmlDocument();

            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            var configuration = doc.CreateElement(string.Empty, "argos-configuration", string.Empty);
            doc.AppendChild(configuration);

            var arena = doc.CreateElement(string.Empty, "arena", string.Empty);
            arena.SetAttribute("center", "0,0,0");
            configuration.AppendChild(arena);

            List<Bounds> boundsList = new List<Bounds>();
            
            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var argosTagForObject = arenaGameObject.GetComponent<ArgosInfo>().tag;

                if (!_parsers.ContainsKey(argosTagForObject)) continue;

                var parser = _parsers[argosTagForObject];
                foreach (var xmlElement in parser.GetXMLElements(doc, arenaGameObject))
                {
                    arena.AppendChild(xmlElement);
                }

                boundsList.Add(parser.GetBounds(arenaGameObject));
            }

            Bounds summedBounds = boundsList.First();

            foreach (var bounds in boundsList.GetRange(1, boundsList.Count-1))
            {
                summedBounds.Encapsulate(bounds);
            }
            

            arena.SetAttribute("center",
                ArgosHelper.VectorToArgosVector(summedBounds.center));
            arena.SetAttribute("size", ArgosHelper.VectorToArgosVector(new Vector3(
                summedBounds.extents.x * 2,
                summedBounds.extents.y * 2,
                summedBounds.extents.z * 2 -x
            )));


            Debug.Log(doc.OuterXml);
        }
    }
}