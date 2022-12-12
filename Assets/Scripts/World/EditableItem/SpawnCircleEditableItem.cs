using System.Collections.Generic;

namespace World.EditableItem
{
    public class SpawnCircleEditableItem : EditableItem
    {
        public bool isSpawnCircle;

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