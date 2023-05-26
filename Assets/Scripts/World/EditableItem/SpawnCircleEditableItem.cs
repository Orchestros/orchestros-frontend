using System.Collections.Generic;
using UnityEngine;

namespace World.EditableItem
{
    // This class represents an editable item with a spawn circle attribute
    public class SpawnCircleEditableItem : EditableItem
    {
        // A boolean flag indicating whether the item is a spawn circle or not
        public bool isSpawnCircle;

        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Create a new dictionary with the spawn circle value
            return new Dictionary<string, string>
            {
                {
                    "spawn_circle", (isSpawnCircle ? 1 : 0).ToString()
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // If the given dictionary does not contain a spawn circle value, return
            if (!newValues.ContainsKey("spawn_circle"))
            {
                return;
            }

            // Parse the new spawn circle value and update the isSpawnCircle flag accordingly
            int.TryParse(newValues["spawn_circle"], out var spawnCircleNewValue);
            
            // If is spawn circle, change material color to green, put it closer to the ground.
            if (spawnCircleNewValue != 0)
            {
                var materialColor = Color.green;
                // Make it opacity 50%
                materialColor.a = 0.5f;
                GetComponent<Renderer>().material.color = materialColor;
                transform.Translate(0, -0.1f, 0);
            }
            isSpawnCircle = spawnCircleNewValue != 0;
        }
    }
}