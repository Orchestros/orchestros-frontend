using System;
using System.Collections.Generic;
using System.Linq;
using Managers.DynamicLine;
using Unity.VisualScripting;
using UnityEngine;
using World.Arena;
using Object = UnityEngine.Object;

namespace Managers
{
    /// <summary>
    /// Manages the arena objects and their highlightable properties.
    /// </summary>
    public class ArenaObjectsManager : MonoBehaviourWithState
    {
        [SerializeField] public DynamicLineManager dynamicLineManager; // the dynamic line manager
        [SerializeField] private Material circleMaterial; // the material for the highlightable circles

        private readonly Dictionary<int, ArenaObject>
            _arenaObjects = new Dictionary<int, ArenaObject>(); // a dictionary of arena objects by instance ID

        private readonly Dictionary<int, Highlightable>
            _highlightables = new Dictionary<int, Highlightable>(); // a dictionary of highlightables by instance ID

        private readonly HashSet<GameObject> _objects = new HashSet<GameObject>(); // a set of game objects

        private readonly List<Action<GameObject>>
            _onObjectAddedCallbacks = new(); // a list of callbacks to be called when a new object is added

        private readonly List<Action<GameObject>>
            _onObjectRemovedCallbacks = new(); // a list of callbacks to be called when an object is removed

        /// <summary>
        /// Gets a list of all the game objects currently managed.
        /// </summary>
        public List<GameObject> GetObjects() => _objects.ToList();

        /// <summary>
        /// Gets the highlightable component of a game object.
        /// </summary>
        /// <param name="objectWithHighlightable">The game object to get the highlightable from.</param>
        /// <returns>The highlightable component if it exists, null otherwise.</returns>
        public Highlightable GetHighlightable(GameObject objectWithHighlightable) =>
            _highlightables.TryGetValue(objectWithHighlightable.GetInstanceID(), out var highlightable)
                ? highlightable
                : null;

        /// <summary>
        /// Gets the arena object component of a game object.
        /// </summary>
        /// <param name="objectWithArena">The game object to get the arena object from.</param>
        /// <returns>The arena object component if it exists, null otherwise.</returns>
        public ArenaObject GetArenaObject(GameObject objectWithArena) =>
            _arenaObjects.TryGetValue(objectWithArena.GetInstanceID(), out var arenaObject)
                ? arenaObject
                : null;

        /// <summary>
        /// Called when a new object is added to the arena.
        /// </summary>
        /// <param name="newObject">The game object that was added.</param>
        public void OnObjectAdded(GameObject newObject)
        {
            // Ensure the object is at least 0.2 units above the floor
            var position = newObject.transform.position;
            if (position.y < 0.2f)
            {
                position = new Vector3(position.x, newObject.GetComponent<MeshRenderer>().bounds.extents.y, position.z);
            }

            _objects.Add(newObject);
            newObject.transform.position = position;

            // Ensure the game object has an arena object component
            var arenaObject = newObject.GetOrAddComponent<ArenaObject>();
            if (!_arenaObjects.ContainsKey(newObject.GetInstanceID()))
            {
                _arenaObjects[newObject.GetInstanceID()] = arenaObject;
            }

            // Ensure the game object has a highlightable component
            var highlightable = newObject.GetOrAddComponent<Highlightable>();
            if (!_highlightables.ContainsKey(newObject.GetInstanceID()))
            {
                highlightable.circleMaterial = circleMaterial;
                highlightable.transform.position += new Vector3(0, 0.2f, 0
                );
                _highlightables[newObject.GetInstanceID()] = highlightable;
            }

            // Call the on object added callbacks
            foreach (var callback in _onObjectAddedCallbacks)
            {
                callback(newObject);
            }

            // Deactivate the arena objects manager
            OnDeactivate();
        }

        /// <summary>
        /// Removes a game object from the arena.
        /// </summary>
        /// <param name="objectToRemove">The game object to remove.</param>
        public void RemoveObject(GameObject objectToRemove)
        {
            // Call the on object removed callbacks
            foreach (var callback in _onObjectRemovedCallbacks)
            {
                callback(objectToRemove);
            }

            // Remove the object from the set of managed objects and destroy it
            _objects.Remove(objectToRemove);
            Destroy(objectToRemove);
        }

        /// <summary>
        /// Adds a game object to the arena.
        /// </summary>
        /// <param name="sourceObject">The game object to add.</param>
        public void AddObject(GameObject sourceObject)
        {
            // If the arena objects manager is already active, do nothing
            if (IsActive || !enabled)
            {
                return;
            }

            // Activate the arena objects manager
            OnActivate();

            // Instantiate the game object and add an object adder component to it
            var newObject = Instantiate(sourceObject, sourceObject.transform.position, sourceObject.transform.rotation);
            var objectAdder = newObject.AddComponent<ObjectAdder>();
            objectAdder.dynamicLineManager = dynamicLineManager;
            objectAdder.OnCompleted = () => OnObjectAdded(newObject);
            objectAdder.OnCanceled = OnDeactivate;
        }

        public void Reset()
        {
            int limit = 10000;

            while (_objects.Count > 0 && limit > 0)
            {
                RemoveObject(_objects.First());
                limit--;
            }
        }

        /// <summary>
        /// Adds a callback to be called when a new object is added.
        /// </summary>
        /// <param name="newCallback">The callback to add.</param>
        public void AddOnObjectAddedCallback(Action<GameObject> newCallback) =>
            _onObjectAddedCallbacks.Add(newCallback);

        /// <summary>
        /// Adds a callback to be called when an object is removed.
        /// </summary>
        /// <param name="newCallback">The callback to add.</param>
        public void AddOnObjectRemovedCallback(Action<GameObject> newCallback) =>
            _onObjectRemovedCallbacks.Add(newCallback);

        /// <inheritdoc />
        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return activeStates.All(x => x != typeof(EditFormManager) && x != typeof(ArgosFileLoader));
        }
    }
}