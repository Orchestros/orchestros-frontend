using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace World.EditableItem
{
    public class SpawnCircleEditableItem : Arena.EditableItem.EditableItem
    {
        public bool isSpawnCircle = false;

        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "spawn_circle", (isSpawnCircle ? 1 : 0).ToString()
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            if (!newValues.ContainsKey("spawn_circle")) return;

            int.TryParse(newValues["spawn_circle"], out var spawnCircleNewValue);
            isSpawnCircle = spawnCircleNewValue != 0;
        }
    }
}