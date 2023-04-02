using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    // This class represents an editable item with width and height attributes
    public class WidthAndHeightEditableItem : EditableItem
    {
        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Get the current local scale of the game object
            var localScale = gameObject.transform.localScale;

            // Create a new dictionary with the length and width values
            return new Dictionary<string, string>
            {
                {
                    "length", localScale.x.ToString(CultureInfo.CurrentCulture)
                },
                {
                    "width", localScale.z.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current local scale of the game object
            var localScale = gameObject.transform.localScale;

            // Initialize the new length and width values to the current values of the local scale
            var newX = localScale.x;
            var newZ = localScale.z;

            // If a new length value is provided, parse it and update the length
            if (newValues.ContainsKey("length"))
            {
                newX = float.Parse(newValues["length"]);
            }

            // If a new width value is provided, parse it and update the width
            if (newValues.ContainsKey("width"))
            {
                newZ = float.Parse(newValues["width"]);
            }

            // Update the local scale of the game object with the new length and width values
            gameObject.transform.localScale = new Vector3(newX, localScale.y, newZ);
        }
    }
}