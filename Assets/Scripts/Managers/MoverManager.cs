using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class MoverManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;

        public float speed = 0.5f;
        public float stepSpeed = 10f;

        private void Update()
        {
            if (!Input.GetKey(KeyCode.LeftControl)) return;

            var deltaVector = Input.GetKey(KeyCode.LeftShift) ? Mover.RetrieveDeltaOneTime(stepSpeed) : Mover.RetrieveDeltaContinuously(speed);

            foreach (var selectedItem in selectionManager.GetSelectedItems())
            {
                selectedItem.transform.position += deltaVector;
            }
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedItems().Count > 0;
        }
    }
}