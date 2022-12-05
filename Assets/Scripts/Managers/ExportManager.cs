using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using World.Arena.EditableItem;
using XML;

namespace Managers
{
    public class ExportManager : MonoBehaviour
    {
        public ArenaObjectsManager arenaObjectsManager;


        private void Update()
        {
            var xmlDocument = new XmlDocument();
            
            if (!Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.LeftControl))

                return;


            foreach (var arenaObjectToXml in arenaObjectsManager.GetObjects()
                         .Select(x => x.GetComponent<ArenaObjectToXml>()))
            {
                foreach (var xmlElement in arenaObjectToXml.GetXMLElements(xmlDocument))
                    xmlDocument.AppendChild(xmlElement);
            }
            
            Debug.Log(xmlDocument.OuterXml);
        }
    }
}