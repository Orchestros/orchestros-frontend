using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages the deletion of objects in the arena.
    /// </summary>
    public class DeleteManager : MonoBehaviourWithState
    {
        [SerializeField] private SelectionManager selectionManager; // the selection manager
        [SerializeField] private ArenaObjectsManager arenaObjectsManager; // the arena objects manager

        /// <summary>
        /// Called once per frame to check for deletion input.
        /// </summary>
        private void Update()
        {
            // If the delete key is not pressed, do nothing
            if (!Input.GetKey(KeyCode.Delete))
            {
                return;
            }

            // Get the currently selected items and clear the selection
            var selectedItems = selectionManager.GetSelectedEditableItems();
            selectionManager.ClearSelection();

            // Remove each selected item from the arena
            foreach (var selectedItem in selectedItems)
            {
                arenaObjectsManager.RemoveObject(selectedItem);
            }
        }

        /// <inheritdoc />
        public override bool ShouldBeEnabled(HashSet<Type> activeStates) =>
            !activeStates.Contains(typeof(EditFormManager)) &&
            selectionManager.GetSelectedEditableItems().Count > 0;
    }
}
