using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

namespace World.Arena.EditableItem
{
    public class RadiusEditableItem : EditableItem
    {
        public override Dictionary<string, string> GetEditableValues()
        {
            var localScale = gameObject.transform.localScale;

            return new Dictionary<string, string>
            {
                {
                    "radius", localScale.x.ToString(CultureInfo.CurrentCulture)
                },
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var localScale = gameObject.transform.localScale;

            var newRadius = localScale.x;

            if (newValues.ContainsKey("radius"))
            {
                newRadius = float.Parse(newValues["radius"]);
            }

            gameObject.transform.localScale = new Vector3(newRadius, localScale.y, newRadius);
        }
    }
}