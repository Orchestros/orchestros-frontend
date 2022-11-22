using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DeleteManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;

        public ArenaObjectsManager arenaObjectsManager;
        private void Update()
        {
            if (!Input.GetKey(KeyCode.Delete)) return;

            var selectedItems = selectionManager.GetSelectedItems();
            
            selectionManager.ClearSelection();

            foreach (var selectedItem in selectedItems)
            {
                arenaObjectsManager.RemoveObject(selectedItem);
            }

        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return !activeStates.Contains(typeof(EditFormManager)) && selectionManager.GetSelectedItems().Count > 0;
        }
    }
}