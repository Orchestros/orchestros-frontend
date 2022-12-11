using System;
using System.Collections.Generic;
using Extensions;
using Managers.DynamicLine;
using UnityEngine;
using Utils;
using World.Arena;

namespace Managers
{
    public class MoverManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;
        public DynamicLineManager dynamicLineManager;

        public float speed = 0.5f;
        public float stepSpeed = 10f;
        public float rotationSpeedInDegrees = 2.5f;
        public bool isMouseDragged;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                OnMouseDown();
                return;
            }

            if (isMouseDragged)
            {
                foreach (var selectedItem in selectionManager.GetSelectedEditableItems())
                {
                    var bounds = selectedItem.GetComponent<Renderer>().bounds;

                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (!Physics.Raycast(ray, out var hit)) continue;
                    
                    bounds.center = hit.point;
                    var newPosition = DynamicLineMoverHelper.RetrieveNewPosition(dynamicLineManager,
                        hit.point, bounds, selectedItem);
                    newPosition.y = selectedItem.transform.position.y;
                    selectedItem.transform.position = newPosition.Round();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnDisable();
                isMouseDragged = false;
                return;
            }

            if (!Input.GetKey(KeyCode.LeftControl)) return;

            var deltaVector = Input.GetKey(KeyCode.LeftShift)
                ? Mover.RetrieveDeltaOneTime(stepSpeed)
                : Mover.RetrieveDeltaContinuously(speed);

            foreach (var selectedItem in selectionManager.GetSelectedEditableItems())
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

        private void OnDisable()
        {
            dynamicLineManager.ClearLinesAndTexts();
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedEditableItems().Count > 0 && !activeStates.Contains(typeof(EditFormManager));
        }

        private void OnMouseDown()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var raycast = Physics.Raycast(ray, out var hit);

            if (!raycast && !hit.collider.gameObject) return;


            var componentInParent = hit.collider.gameObject.GetComponentInParent<ArenaObject>();

            if (!componentInParent)
            {
                return;
            }
            
            var colliderGameObject = componentInParent.gameObject;

            if (selectionManager.GetSelectedEditableItems().Contains(colliderGameObject))
            {
                isMouseDragged = true;
            }
        }
    }
}