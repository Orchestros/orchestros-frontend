using System.Collections.Generic;
using System.Globalization;
using Vector3 = UnityEngine.Vector3;

namespace World.EditableItem
{
    public class LightEditableItem : Arena.EditableItem.EditableItem
    {
        private float _intensity;
        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "intensity", _intensity.ToString(CultureInfo.CurrentCulture)
                },
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