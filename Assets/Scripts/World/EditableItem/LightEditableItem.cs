using System.Collections.Generic;
using System.Globalization;

namespace World.EditableItem
{
    public class LightEditableItem : EditableItem
    {
        private float _intensity;
        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "intensity", _intensity.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var localScale = gameObject.transform.localScale;

            var newIntensity = localScale.x;

            if (newValues.ContainsKey("intensity"))
            {
                newIntensity = float.Parse(newValues["intensity"]);
            }

            _intensity = newIntensity;
        }
    }
}