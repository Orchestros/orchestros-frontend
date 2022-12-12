using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    public class RadiusEditableItem : EditableItem
    {
        public override Dictionary<string, string> GetEditableValues()
        {
            var localScale = gameObject.transform.localScale;

            return new Dictionary<string, string>
            {
                {
                    "radius", (localScale.x / 2).ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var localScale = gameObject.transform.localScale;

            var newRadius = localScale.x;

            if (newValues.ContainsKey("radius")) newRadius = float.Parse(newValues["radius"]) * 2;

            gameObject.transform.localScale = new Vector3(newRadius, localScale.y, newRadius);
        }
    }
}