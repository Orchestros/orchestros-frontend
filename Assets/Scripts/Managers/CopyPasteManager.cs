using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers
{
    /// <summary>
    /// Manages the copy-pasting of objects in the arena.
    /// </summary>
    public class CopyPasteManager : MonoBehaviourWithState
    {
        [SerializeField] private SelectionManager selectionManager; // the selection manager
        [SerializeField] private ArenaObjectsManager arenaObjectsManager; // the arena objects manager

        private List<GameObject> _copiedObjects = new List<GameObject>(); // a list of copied objects
        private GameObject _group; // the copy-paste group

        /// <summary>
        /// Called once per frame to check for copy-pasting input.
        /// </summary>
        private void Update()
        {
            // If the control key is not held down or the copy-paste manager is already active, do nothing
            if (!Input.GetKey(KeyCode.LeftControl) || IsActive)
            {
                return;
            }

            // If the 'C' key is pressed, copy the currently selected objects
            if (Input.GetKey(KeyCode.C))
            {
                _copiedObjects = selectionManager.GetSelectedEditableItems();
            }
            // If the 'V' key is pressed and there are copied objects, paste them into the arena
            else if (Input.GetKey(KeyCode.V) && _copiedObjects.Count > 0)
            {
                OnActivate();
                _group = new GameObject();

                // Calculate the position of the first copied object
                var firstObjectPosition = _copiedObjects.First().transform.position;

                // Instantiate and parent the copied objects to the copy-paste group
                foreach (var newObject in _copiedObjects.Select(copiedObject =>
                             Instantiate(
                                 copiedObject,
                                 copiedObject.transform.position - firstObjectPosition,
                                 copiedObject.transform.rotation,
                                 _group.transform
                             )))
                {
                    newObject.transform.parent = _group.transform;
                }

                // Add an object adder component to the copy-paste group to handle collisions
                var objectAdder = _group.AddComponent<ObjectAdder>();
                objectAdder.dynamicLineManager = arenaObjectsManager.dynamicLineManager;
                objectAdder.OnCompleted = OnCopyFinishes;
                objectAdder.OnCanceled = OnDeactivate;
            }
        }

        /// <summary>
        /// Called when the copy-pasting is complete.
        /// </summary>
        private void OnCopyFinishes()
        {
            // Add the pasted objects to the arena
            for (var i = 0; i < _group.transform.childCount; i++)
            {
                var childObject = _group.transform.GetChild(i).gameObject;
                arenaObjectsManager.OnObjectAdded(childObject);
            }

            // Detach the child objects from the copy-paste group, destroy the group, and deactivate the manager
            _group.transform.DetachChildren();
            Destroy(_group);
            OnDeactivate();
        }

        /// <inheritdoc />
        public override bool ShouldBeEnabled(HashSet<Type> activeStates) =>
            !activeStates.Contains(typeof(EditFormManager)) &&
            !activeStates.Contains(typeof(ArgosFileLoader)) &&
            selectionManager.GetSelectedEditableItems().Count > 0;
    }
}