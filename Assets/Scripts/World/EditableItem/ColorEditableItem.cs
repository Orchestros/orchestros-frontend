using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

namespace World.EditableItem
{
    public class ColorEditableItem : EditableItem
    {
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }


        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "color", "#" + _renderer.material.color.ToHexString()[..6]
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var color = _renderer.material.color;

            if (newValues.ContainsKey("color"))
            {
                ColorUtility.TryParseHtmlString(newValues["color"], out color);
            }

            _renderer.material.color = color;
        }

        public string GetColor()
        {
            return _renderer.material.color.ToHexString();
        }
    }
}