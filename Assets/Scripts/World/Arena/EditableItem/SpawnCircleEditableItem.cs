using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.Arena.EditableItem
{
    public class SpawnCircleEditableItem : EditableItem
    {
        public override Dictionary<string, string> GetEditableValues()
        {
            var spawnCircle = GetComponent<SpawnCircle>();

            return new Dictionary<string, string>
            {
                {
                    "spawn_circle", (spawnCircle == null ? 0 : 1).ToString()
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            if (!newValues.ContainsKey("spawn_circle")) return;

            var spawnCircle = GetComponent<SpawnCircle>();

            if (int.Parse(newValues["spawn_circle"]) != 0)
            {
                if (spawnCircle == null)
                {
                    gameObject.AddComponent<SpawnCircle>();
                }
            }
            else if (spawnCircle != null)
            {
                Destroy(spawnCircle);
            }
        }
    }
}