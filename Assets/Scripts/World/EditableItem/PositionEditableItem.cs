using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.Arena.EditableItem
{
    public class PositionEditableItem : EditableItem
    {
        public override Dictionary<string, string> GetEditableValues()
        {
            var position = transform.position;

            return new Dictionary<string, string>
            {
                {
                    "x", position.x.ToString(CultureInfo.CurrentCulture)
                },
                {
                    "z", position.z.ToString(CultureInfo.CurrentCulture)
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            var position = gameObject.transform.position;

            var newX = position.x;
            var newZ = position.z;

            if (newValues.ContainsKey("x"))
            {
                newX = float.Parse(newValues["x"]);
            }

            if (newValues.ContainsKey("z"))
            {
                newZ = float.Parse(newValues["z"]);
            }

            gameObject.transform.position = new Vector3(newX, position.y, newZ);
        }
    }
}