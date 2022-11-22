using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Arena.EditableItem
{
    public abstract class EditableItem : MonoBehaviour
    {
        public abstract Dictionary<string, string> GetEditableValues();
        public abstract void UpdateValues(Dictionary<string, string> newValues);
    }
}