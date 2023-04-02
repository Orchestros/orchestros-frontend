using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Extensions;
using Managers.DynamicLine;
using World.Arena;

namespace Managers
{
    // A manager for moving objects in the game world.
    public class MoverManager : MonoBehaviourWithState
    {
        // A manager for selecting objects in the game world.
        public SelectionManager selectionManager;
        // A manager for drawing dynamic lines in the game world.
        public DynamicLineManager dynamicLineManager;

        // The speed at which objects move.
        public float speed = 0.5f;
        // The speed at which objects move when holding the shift key.
        public float stepSpeed = 10f;
        // The speed at which objects rotate.
        public float rotationSpeedInDegrees = 2.5f;

        // A flag indicating whether the mouse is being dragged.
        public bool isMouseDragged;

        // The main camera in the scene.
        private Camera _camera;

        // Called once when the script is first enabled.
        private void Start()
        {
            // Get the main camera in the scene.
            _camera = Camera.main;
        }

        // Called once per frame.
        private void Update()
        {
            // If the left mouse button is pressed down, call the OnMouseDown() method.
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
                return;
            }

            // If the mouse is currently being dragged, move the selected objects.
            if (isMouseDragged)
            {
                foreach (var selectedItem in selectionManager.GetSelectedEditableItems())
                {
                    // Get the bounds of the selected item.
                    var bounds = selectedItem.GetComponent<Renderer>().bounds;

                    // Cast a ray from the mouse position to the game world.
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    // If the ray does not intersect with any objects in the game world, skip to the next iteration of the loop.
                    if (!Physics.Raycast(ray, out var hit)) continue;

                    // Move the bounds to the intersection point.
                    bounds.center = hit.point;

                    // Get the new position of the selected item.
                    var newPosition = DynamicLineMoverHelper.RetrieveNewPosition(dynamicLineManager, hit.point, bounds, selectedItem);
                    // Set the Y position of the new position to the Y position of the selected item.
                    newPosition.y = selectedItem.transform.position.y;
                    // Move the selected item to the new position rounded to the nearest integer.
                    selectedItem.transform.position = newPosition.Round();
                }
            }

            // If the left mouse button is released, call the OnDisable() method and set isMouseDragged to false.
            if (Input.GetMouseButtonUp(0))
            {
                OnDisable();
                isMouseDragged = false;
                return;
            }

            // If the left control key is not being held down, skip to the next iteration of the loop.
            if (!Input.GetKey(KeyCode.LeftControl)) return;

            // Get the delta vector for moving the selected items.
            var deltaVector = Input.GetKey(KeyCode.LeftShift) ? Mover.RetrieveDeltaOneTime(stepSpeed) : Mover.RetrieveDeltaContinuously(speed);

            // Move the selected items by the delta vector.
            foreach (var selectedItem in selectionManager.GetSelectedEditableItems())
            {
                selectedItem.transform.position += deltaVector;

                // If the mouse wheel is scrolled up, rotate the selected item to the right.
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    selectedItem.transform.Rotate(Vector3.up * rotationSpeedInDegrees, Space.Self);

                // If the mouse wheel is scrolled down, rotate the selected item to the left.
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    selectedItem.transform.Rotate(Vector3.down * rotationSpeedInDegrees, Space.Self);
            }
        }
        
        // Called when the script is disabled.
        private void OnDisable()
        {
            // Clear all dynamic lines and texts.
            dynamicLineManager.ClearLinesAndTexts();
        }

        // Called when the left mouse button is pressed down.
        private void OnMouseDown()
        {
            // Cast a ray from the mouse position to the game world.
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            // Check if the ray intersects with any objects in the game world.
            var raycast = Physics.Raycast(ray, out var hit);

            // If the ray does not intersect with any objects in the game world, skip to the next iteration of the loop.
            if (!raycast && !hit.collider.gameObject) return;

            // Get the ArenaObject component of the parent object of the object that was hit by the ray.
            var componentInParent = hit.collider.gameObject.GetComponentInParent<ArenaObject>();

            // If the object does not have an ArenaObject component, skip to the next iteration of the loop.
            if (!componentInParent) return;

            // Get the game object of the ArenaObject component.
            var colliderGameObject = componentInParent.gameObject;

            // If the game object is already selected, set isMouseDragged to true.
            if (selectionManager.GetSelectedEditableItems().Contains(colliderGameObject)) isMouseDragged = true;
        }

        // Called to determine if the script should be enabled.
        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            // Enable the script if there is at least one selected editable item and the EditFormManager state is not active.
            return selectionManager.GetSelectedEditableItems().Count > 0 && !activeStates.Contains(typeof(EditFormManager));
        }
    }
}

    
