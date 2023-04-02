using UnityEngine;

namespace World.Arena
{
    // This class represents an object in the arena
    public class ArenaObject : MonoBehaviour
    {
        // A boolean flag indicating whether the object is locked and cannot be edited
        private bool _isLocked;

        // A public property that returns true if the object can be edited (i.e., it is not locked)
        public bool CanBeEdited => !_isLocked;

        // This function changes the lock state of the object to the given state
        public void ChangeLock(bool newLockState)
        {
            _isLocked = newLockState;
        }
    }
}