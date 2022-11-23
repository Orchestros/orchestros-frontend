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
        public float relativeDragSpeed = 2.5f;
        public bool isMouseDragged = false;
        private Camera _camera;
        private Vector3 _dragOrigin;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                _dragOrigin = Input.mousePosition;
                OnMouseDown();
                return;
            }

            if (isMouseDragged)
            {
                var delta = Dragger.RetrieveDragDelta(_camera, _dragOrigin, relativeDragSpeed);
                _dragOrigin = Input.mousePosition;
                foreach (var selectedItem in selectionManager.GetSelectedItems())
                {
                    selectedItem.transform.Translate(-delta);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDragged = false;
                return;
            }

            if (!Input.GetKey(KeyCode.LeftControl)) return;

            var deltaVector = Input.GetKey(KeyCode.LeftShift)
                ? Mover.RetrieveDeltaOneTime(stepSpeed)
                : Mover.RetrieveDeltaContinuously(speed);

            foreach (var selectedItem in selectionManager.GetSelectedItems())
            {
                selectedItem.transform.position += deltaVector;

                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    selectedItem.transform.Rotate(Vector3.up * rotationSpeedInDegrees, Space.Self);
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    selectedItem.transform.Rotate(Vector3.down * rotationSpeedInDegrees, Space.Self);
                }
            }
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedItems().Count > 0;
        }

        private void OnMouseDown()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var raycast = Physics.Raycast(ray, out var hit);

            if (!raycast) return;

            var colliderGameObject = hit.collider.gameObject;

            if (selectionManager.GetSelectedItems().Contains(colliderGameObject))
            {
                isMouseDragged = true;
            }
        }
    }
}