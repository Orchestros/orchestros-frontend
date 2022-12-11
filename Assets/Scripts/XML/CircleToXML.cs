using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using World.EditableItem;

namespace XML
{
    public class Circle : ArenaObjectToXml
    {
        public override List<XmlElement> GetXMLElements(XmlDocument document)
        {
            var node = document.CreateElement(string.Empty, "circle", string.Empty);
            var localScale = transform.localScale;

            node.SetAttribute("id", gameObject.GetInstanceID().ToString());
            node.SetAttribute("radius", (localScale.x/2).ToString(CultureInfo.InvariantCulture)); 
            node.SetAttribute("movable", "false");
            
            var spawnCircleEditableItem = gameObject.GetComponent<SpawnCircleEditableItem>();
            var isEditableCircle = false;

            if (spawnCircleEditableItem)
            {
                isEditableCircle = spawnCircleEditableItem.isSpawnCircle;
            }
            
            node.SetAttribute("spawn_circle", isEditableCircle ? "true" :"false");

            ArgosHelper.InsertBodyTagFromTransform(document, node, transform);

            return new List<XmlElement> { node };
        }
    }
}