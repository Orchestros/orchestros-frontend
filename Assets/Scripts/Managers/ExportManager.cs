using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using XML;

namespace Managers
{
    public class ExportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;


        private void Update()
        {
            var doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            var configuration = doc.CreateElement(string.Empty, "argos-configuration", string.Empty);
            doc.AppendChild(configuration);

            var arena = doc.CreateElement(string.Empty, "arena", string.Empty);
            arena.SetAttribute("center", "0,0,0");
            configuration.AppendChild(arena);

            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))

                return;


            Bounds bounds = new Bounds();

            foreach (var arenaGameObject in arenaObjectsManager.GetObjects())
            {
                var arenaObjectToXml = arenaGameObject.GetComponent<ArenaObjectToXml>();
                if (!arenaObjectToXml) continue;

                bounds.Encapsulate(arenaGameObject.GetComponent<Renderer>().bounds);

                foreach (var xmlElement in arenaObjectToXml.GetXMLElements(doc))
                    arena.AppendChild(xmlElement);
            }

            arena.SetAttribute("center", ArgosHelper.VectorToArgosVector(bounds.center));
            arena.SetAttribute("size", ArgosHelper.VectorToArgosVector(bounds.extents * 2));


            Debug.Log(doc.OuterXml);
        }
    }
}