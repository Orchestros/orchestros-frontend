using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Serialization;

namespace World.EditableItem
{
    // This class represents an editable item with a light intensity attribute
    public class LightEditableItem : EditableItem
    {
        // The intensity of the light
        public float intensity;

        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Create a new dictionary with the current intensity value
            return new Dictionary<string, string>
            {
                {
                    "intensity", intensity.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current local scale of the game object
            var localScale = gameObject.transform.localScale;

            // Initialize the new intensity to the x-axis value of the local scale
            var newIntensity = localScale.x;

            // If a new intensity value is provided, parse it and update the intensity
            if (newValues.ContainsKey("intensity"))
            {
                newIntensity = float.Parse(newValues["intensity"]);
            }

            intensity = newIntensity;
        }
    }
}