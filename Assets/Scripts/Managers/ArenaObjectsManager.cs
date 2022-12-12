using System;
using System.Collections.Generic;
using System.Linq;
using Managers.DynamicLine;
using Unity.VisualScripting;
using UnityEngine;
using World.Arena;

namespace Managers
{
    public class ArenaObjectsManager : MonoBehaviourWithState
    {
        private readonly HashSet<GameObject> _objects = new();

        private readonly Dictionary<int, Highlightable> _highlightable = new();
        private readonly Dictionary<int, ArenaObject> _arenaObjects = new();

        private readonly List<Action<GameObject>> _onObjectAddedCallbacks = new();
        private readonly List<Action<GameObject>> _onObjectRemovedCallbacks = new();

        public Material circleMaterial;

        public DynamicLineManager dynamicLineManager;

        public List<GameObject> GetObjects()
        {
            return _objects.ToList();
        }

        public Highlightable GetHighlightable(GameObject objectWithHighlightable)
        {
            return _highlightable[objectWithHighlightable.GetInstanceID()];
        }
        public ArenaObject GetArenaObject(GameObject objectWithArena)
        {
            return _arenaObjects[objectWithArena.GetInstanceID()];
        }

        public void OnObjectAdded(GameObject newObject)
        {
            _objects.Add(newObject);
            newObject.GetOrAddComponent<ArenaObject>();


    
            AddHighlightableToGameObject(newObject);
            AddArenaObjectToGameObject(newObject);
            
            foreach (var callback in _onObjectAddedCallbacks)
            {
                callback(newObject);
            }

            OnDeactivate();
        }

        private void AddHighlightableToGameObject(GameObject newObject)
        {
            if (_highlightable.ContainsKey(newObject.GetInstanceID())) return; // if it already  exists, do nothing
            var highlightable = newObject.GetOrAddComponent<Highlightable>();
            highlightable.circleMaterial = circleMaterial;
            highlightable.transform.position += new Vector3(0, 0.2f, 0);
            _highlightable[newObject.GetInstanceID()] = highlightable;
        }
        private void AddArenaObjectToGameObject(GameObject newObject)
        {
            if (_arenaObjects.ContainsKey(newObject.GetInstanceID())) return; // if it already  exists, do nothing
            var arenaObject = newObject.GetOrAddComponent<ArenaObject>();
            _arenaObjects[newObject.GetInstanceID()] = arenaObject;
        }

        public void RemoveObject(GameObject objectToRemove)
        {
            foreach (var callback in _onObjectRemovedCallbacks)
            {
                callback(objectToRemove);
            }

            _objects.Remove(objectToRemove);
            Destroy(objectToRemove);
        }

        public void AddObject(GameObject sourceObject)
        {
            if (IsActive) return;

            OnActivate();

            var newObject = Instantiate(
                sourceObject,
                sourceObject.transform.position,
                sourceObject.transform.rotation
            );

            var objectAdder = newObject.AddComponent<ObjectAdder>();
            objectAdder.dynamicLineManager = dynamicLineManager;
            objectAdder.OnCompleted = () => OnObjectAdded(newObject);
            objectAdder.OnCanceled = OnDeactivate;
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            var prohibitedStates = new HashSet<Type>
            {
                typeof(EditFormManager)
            };

            return !activeStates.Any(x => prohibitedStates.Contains(x));
        }

        public void addOnObjectAddedCallback(Action<GameObject> newCallback)
        {
            _onObjectAddedCallbacks.Add(newCallback);
        }

        public void addOnObjectRemovedCallback(Action<GameObject> newCallback)
        {
            _onObjectRemovedCallbacks.Add(newCallback);
        }
    }
}