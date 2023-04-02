using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LockerManger : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;
        public ArenaObjectsManager arenaObjectsManager;

        private void Update()
        {
            // Check if the control and L keys are pressed
            if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKey(KeyCode.L)) return;

            // Check if the shift key is pressed to unlock items instead of locking them
            var isUnlockCommand = Input.GetKey(KeyCode.LeftShift);

            // Lock or unlock each selected item
            foreach (var selectedItem in selectionManager.GetSelectedItems())
                arenaObjectsManager.GetArenaObject(selectedItem).ChangeLock(!isUnlockCommand);

            // Clear the selection to prevent multiple locking or unlocking of the same items
            selectionManager.ClearSelection();
        }

        /// <inheritdoc />
        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            // The manager should be enabled if there are selected items
            return selectionManager.GetSelectedItems().Count > 0;
        }
    }
}