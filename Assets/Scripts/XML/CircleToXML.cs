using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

namespace XML
{
    public class CircleToXML : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Circle;

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var node = document.CreateElement(string.Empty, "circle", string.Empty);
            var localScale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x/2)); 
            node.SetAttribute("movable", "false");
            
            var spawnCircleEditableItem = arenaObject.GetComponent<SpawnCircleEditableItem>();
            var isEditableCircle = false;

            if (spawnCircleEditableItem)
            {
                isEditableCircle = spawnCircleEditableItem.isSpawnCircle;
            }
            
            node.SetAttribute("spawn_circle", isEditableCircle ? "true" :"false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, arenaObject.transform);

            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}