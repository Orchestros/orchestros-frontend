using UnityEngine;

namespace Managers
{
    public class ContinuousAddObjectManager : MonoBehaviour

    {
        [Tooltip("The GameObject that serves as a template for the new object to be added to the game arena.")]
        public GameObject prefab;

        [Tooltip("The instance of the ArenaObjectsManager class, which manages the objects in the game arena.")]
        public ArenaObjectsManager manager;


        private bool _isCapsLockOn;

        private void Start()
        {
            manager.AddOnObjectAddedCallback((_) =>
            {
                // if in cap lock mode
                if (!_isCapsLockOn) return;

                Debug.Log("Object added");
                // Delay call by 10ms to prevent  collision with the object
                Invoke(nameof(AddObject), 0.01f);
            });
        }

        private void Update()
        {
            // If cap lock is pressed down
            if (!Input.GetKeyDown(KeyCode.CapsLock)) return;
            
            _isCapsLockOn = !_isCapsLockOn;
            
            if (_isCapsLockOn)
            {
                AddObject();
            }
        }

        private void AddObject()
        {
            manager.AddObject(prefab);
        }
    }
}