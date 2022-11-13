using System;
using UnityEngine;

namespace Managers
{
    public class SceneManager : MonoBehaviour
    {
        public ObjectManager objectManager;
        public SelectionManager selectionManager;

        
        
        private void Start()
        {
            objectManager.activationEvent.AddListener(OnStateActivationChanged);
        }

        private void OnStateActivationChanged(bool isActive)
        {
            selectionManager.enabled = !isActive;
            Debug.Log(selectionManager.enabled);
        }
    }
}
