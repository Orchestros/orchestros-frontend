using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using World.EditableItem;

namespace Managers.Argos.XML
{
    public class ArgosCircleLink : ArenaObjectToXml
    {
        public override ArgosTag Tag => ArgosTag.Circle;

        public override GameObject InstantiateFromElement(XmlElement element)
        {
            var newObject = Instantiate(prefab);
            var radius = ArgosHelper.StringToFloatWithInverseArgosFactor(element.GetAttribute("radius"));
            newObject.transform.localScale = new Vector3(radius*2, 1, radius*2);
            Debug.Log("x");
            var position = ArgosHelper.ArgosVectorToVector(element.GetAttribute("position"));
            Debug.Log(position);

            position += new Vector3(0, 1, 0);
            newObject.transform.position = position;
            var spawnCircleEditableItem = newObject.GetComponent<SpawnCircleEditableItem>();
            spawnCircleEditableItem.UpdateValues(new Dictionary<string, string>
            {
                {
                    "spawn_circle", (element.Name == "spawnCircle" ? 1 : 0).ToString()
                }
            });

            return newObject;
        }

        public override List<XmlElement> GetXMLElements(XmlDocument document, GameObject arenaObject)
        {
            var isEditableCircle = false;
            var spawnCircleEditableItem = arenaObject.GetComponent<SpawnCircleEditableItem>();

            if (spawnCircleEditableItem) isEditableCircle = spawnCircleEditableItem.isSpawnCircle;


            var node = document.CreateElement(string.Empty, isEditableCircle ? "spawnCircle" : "circle", string.Empty);
            var localScale = arenaObject.transform.localScale;

            node.SetAttribute("id", arenaObject.GetInstanceID().ToString());
            node.SetAttribute("radius", ArgosHelper.FloatToStringWithArgosFactor(localScale.x / 2));

            node.SetAttribute("color", "black");

            node.SetAttribute("position", ArgosHelper.VectorToArgosVectorNoHeight(arenaObject.transform.position));
            node.SetAttribute("spawn_circle", isEditableCircle ? "true" : "false");


            return new List<XmlElement> { node };
        }

        public override Bounds GetBounds(GameObject arenaObject)
        {
            return arenaObject.GetComponent<Renderer>().bounds;
        }
    }
}