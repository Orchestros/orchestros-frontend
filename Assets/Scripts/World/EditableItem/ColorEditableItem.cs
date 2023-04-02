using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

namespace World.EditableItem
{
    // This class represents an editable item with a color attribute
    public class ColorEditableItem : EditableItem
    {
        // The renderer component for this item
        private Renderer _renderer;

        // This function is called when the script is enabled
        private void Start()
        {
            // Get the renderer component
            _renderer = GetComponent<Renderer>();
        }

        // This function returns a dictionary of editable values for the item
        public override Dictionary<string, string> GetEditableValues()
        {
            // Create a new dictionary with the current color value
            return new Dictionary<string, string>
            {
                {
                    "color", "#" + _renderer.material.color.ToHexString()[..6]
                }
            };
        }

        // This function updates the item's editable values with the values in the given dictionary
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Get the current color value
            var color = _renderer.material.color;

            // If a new color value is provided, update the color
            if (newValues.ContainsKey("color"))
            {
                ColorUtility.TryParseHtmlString(newValues["color"], out color);
            }

            _renderer.material.color = color;
        }

        // This function returns the color of the item as a hex string
        public string GetColor()
        {
            return _renderer.material.color.ToHexString();
        }
    }
}
