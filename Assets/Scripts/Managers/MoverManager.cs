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
        public float rotationSpeedInDegrees = 2.5f;

        private void Update()
        {
            if (!Input.GetKey(KeyCode.LeftControl)) return;

            var deltaVector = Input.GetKey(KeyCode.LeftShift) ? Mover.RetrieveDeltaOneTime(stepSpeed) : Mover.RetrieveDeltaContinuously(speed);

            foreach (var selectedItem in selectionManager.GetSelectedItems())
            {
                selectedItem.transform.position += deltaVector;
                
                if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                    selectedItem.transform.Rotate(Vector3.up * rotationSpeedInDegrees, Space.Self);
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                    selectedItem.transform.Rotate(Vector3.down * rotationSpeedInDegrees, Space.Self);
                }
            }
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedItems().Count > 0;
        }
    }
}