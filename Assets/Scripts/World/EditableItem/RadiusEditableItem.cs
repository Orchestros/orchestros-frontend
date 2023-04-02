using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    // This class represents an editable item with a radius attribute
    public class RadiusEditableItem : EditableItem
    {
        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Get the current local scale of the game object
            var localScale = gameObject.transform.localScale;

            // Create a new dictionary with the radius value
            return new Dictionary<string, string>
            {
                {
                    "radius", (localScale.x / 2).ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current local scale of the game object
            var localScale = gameObject.transform.localScale;

            // Initialize the new radius to the current x-axis value of the local scale
            var newRadius = localScale.x;

            // If a new radius value is provided, parse it and update the radius
            if (newValues.ContainsKey("radius"))
            {
                newRadius = float.Parse(newValues["radius"]) * 2;
            }

            // Update the local scale of the game object with the new radius value
            gameObject.transform.localScale = new Vector3(newRadius, localScale.y, newRadius);
        }
    }
}