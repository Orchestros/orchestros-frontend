using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Arena;

namespace Managers
{
    public class CopyPasteManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;

        private List<GameObject> _copiedObjects = new();

        private GameObject _group;

        private void Update()
        {
            if (!Input.GetKey(KeyCode.LeftControl) || IsActive) return;

            if (Input.GetKey(KeyCode.C))
            {
                _copiedObjects = selectionManager.GetSelectedItems();
            }
            else if (Input.GetKey(KeyCode.V) && _copiedObjects.Count > 0)
            {
                OnActivate();

                _group = new GameObject();

                var firstObjectPosition = _copiedObjects.First().transform.position;

                foreach (var newObject in _copiedObjects.Select(copiedObject => Instantiate(
                             copiedObject,
                             copiedObject.transform.position - firstObjectPosition,
                             copiedObject.transform.rotation
                         )))
                {
                    newObject.transform.parent = _group.transform;
                }

                var c = _group.AddComponent<ObjectAdder>();
                c.OnCompleted = OnCopyFinishes;
            }
        }

        private void OnCopyFinishes()
        {
            foreach (var componentsInChild in _group.GetComponentsInChildren<Highlightable>())
            {
                Destroy(componentsInChild);
            }

            _group.transform.DetachChildren();

            Destroy(_group);
            OnDeactivate();
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return !activeStates.Contains(typeof(EditFormManager)) && selectionManager.GetSelectedItems().Count > 0;
        }
    }
}