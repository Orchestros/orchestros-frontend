using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    // This class represents an editable item with a position attribute
    public class RotationEditableItem : EditableItem
    {
        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {

            // Create a new dictionary with the y rotation value (Euler angle)
            return new Dictionary<string, string>
            {
                {
                    "rotation", (transform.rotation.eulerAngles.y).ToString(CultureInfo.CurrentCulture)
                },
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current rotation of the game object
            var rotation = gameObject.transform.rotation;

            // Initialize the new x and z values to the current rotation values
            var newYRotation = rotation.y;

            // If a new rotation value is provided, parse it and update newYRotation
            if (newValues.TryGetValue("rotation", out var value))
            {
                newYRotation = float.Parse(value);
            }

            gameObject.transform.rotation = Quaternion.Euler(0, newYRotation, 0);
        }
    }
}