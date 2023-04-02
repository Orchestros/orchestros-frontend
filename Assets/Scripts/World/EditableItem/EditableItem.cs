using System.Collections.Generic;
using UnityEngine;

namespace World.EditableItem
{
    // This abstract class represents an editable item in the game world
    public abstract class EditableItem : MonoBehaviour
    {
        // This function returns a dictionary of editable values for the item
        public abstract Dictionary<string, string> GetEditableValues();

        // This function updates the item's editable values with the values in the given dictionary
        public abstract void UpdateValues(Dictionary<string, string> newValues);
    }
}
