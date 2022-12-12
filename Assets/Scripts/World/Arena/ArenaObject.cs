using UnityEngine;

namespace World.Arena
{
    public class ArenaObject : MonoBehaviour
    {
        private bool _isLocked;
        public bool CanBeEdited => !_isLocked;

        public void ChangeLock(bool newLockState)
        {
            _isLocked = newLockState;
        }
    }
}