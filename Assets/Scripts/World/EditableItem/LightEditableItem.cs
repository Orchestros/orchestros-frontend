using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Serialization;

namespace World.EditableItem
{
    public class LightEditableItem : EditableItem
    {
        public float intensity;

        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "intensity", intensity.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var localScale = gameObject.transform.localScale;

            var newIntensity = localScale.x;

            if (newValues.ContainsKey("intensity")) newIntensity = float.Parse(newValues["intensity"]);

            intensity = newIntensity;
        }
    }
}