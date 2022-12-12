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
            if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKey(KeyCode.L)) return;

            var isUnlockCommand = Input.GetKey(KeyCode.LeftShift);

            foreach (var selectedItem in selectionManager.GetSelectedItems())
                arenaObjectsManager.GetArenaObject(selectedItem).ChangeLock(!isUnlockCommand);

            selectionManager.ClearSelection();
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedItems().Count > 0;
        }
    }
}