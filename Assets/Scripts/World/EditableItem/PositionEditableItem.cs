using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    // This class represents an editable item with a position attribute
    public class PositionEditableItem : EditableItem
    {
        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Get the current position of the game object
            var position = transform.position;

            // Create a new dictionary with the x and z values of the position
            return new Dictionary<string, string>
            {
                {
                    "x", position.x.ToString(CultureInfo.CurrentCulture)
                },
                {
                    "z", position.z.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current position of the game object
            var position = gameObject.transform.position;

            // Initialize the new x and z values to the current position values
            var newX = position.x;
            var newZ = position.z;

            // If a new x value is provided, parse it and update newX
            if (newValues.ContainsKey("x"))
            {
                newX = float.Parse(newValues["x"]);
            }

            // If a new z value is provided, parse it and update newZ
            if (newValues.ContainsKey("z"))
            {
                newZ = float.Parse(newValues["z"]);
            }

            // Update the position of the game object with the new x and z values
            gameObject.transform.position = new Vector3(newX, position.y, newZ);
        }
    }
}
