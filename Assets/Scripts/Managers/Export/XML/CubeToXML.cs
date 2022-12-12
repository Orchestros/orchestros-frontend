﻿using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers.Export.XML
{
    public class CubeToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Cube;

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "box", string.Empty);
            var localScale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("size", ArgosHelper.VectorToArgosVector(localScale));
            node.SetAttribute("movable", "false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, arenaObject.transform);
            
            return new List<XmlElement> { node };
        }
        
        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
    
}