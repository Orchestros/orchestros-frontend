using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.EditableItem
{
    public class WidthAndHeightEditableItem : EditableItem
    {
        public override Dictionary<string, string> GetEditableValues()
        {
            var localScale = gameObject.transform.localScale;

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

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var localScale = gameObject.transform.localScale;

            var newX = localScale.x;
            var newZ = localScale.z;

            if (newValues.ContainsKey("length")) newX = float.Parse(newValues["length"]);

            if (newValues.ContainsKey("width")) newZ = float.Parse(newValues["width"]);

            gameObject.transform.localScale = new Vector3(newX, localScale.y, newZ);
        }
    }
}