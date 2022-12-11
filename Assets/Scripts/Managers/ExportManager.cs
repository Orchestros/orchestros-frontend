using System.Linq;
using System.Xml;
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

            var element1 = doc.CreateElement(string.Empty, "argos-configuration", string.Empty);
            doc.AppendChild(element1);

            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))

                return;


            foreach (var arenaObjectToXmlList in arenaObjectsManager.GetObjects()
                         .Select(x => x.GetComponents<ArenaObjectToXml>()))
            {
                foreach (var arenaObjectToXml in arenaObjectToXmlList)
                {
                    foreach (var xmlElement in arenaObjectToXml.GetXMLElements(doc))
                        element1.AppendChild(xmlElement);
                }
            }

            Debug.Log(doc.OuterXml);
        }
    }
}