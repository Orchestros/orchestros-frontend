using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Arena;

namespace Managers
{
    public class ArenaObjectsManager : MonoBehaviourWithState
    {
        private readonly HashSet<GameObject> _objects = new();

        private readonly Dictionary<int, Highlightable> _highlightable = new();

        public Texture highlightedTexture;

        public List<GameObject> GetObjects()
        {
            return _objects.ToList();
        }

        public Highlightable GetHighlightable(GameObject instance)
        {
            return _highlightable[instance.GetInstanceID()];
        }

        private void OnObjectAdded(GameObject newObject)
        {
            _objects.Add(newObject);
            newObject.AddComponent<ArenaObject>();
            var highlightable = newObject.AddComponent<Highlightable>();
            highlightable.selectedTexture = highlightedTexture;
            
            _highlightable[newObject.GetInstanceID()] = highlightable;
            
            OnDeactivate();
        }

        public void RemoveObject(GameObject objectToRemove)
        {
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

            newObject.layer = 2; // invisible to raytracing in layer 2 

            var objectAdder = newObject.AddComponent<ObjectAdder>();
            objectAdder.OnCompleted = () => OnObjectAdded(newObject);
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            var prohibitedStates = new HashSet<Type>()
            {
                typeof(EditFormManager)
            };

            return !activeStates.Any(x => prohibitedStates.Contains(x));
        }
    }
}