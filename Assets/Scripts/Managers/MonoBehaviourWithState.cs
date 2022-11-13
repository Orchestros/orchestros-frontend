using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public abstract class MonoBehaviourWithState : MonoBehaviour
    {
        public UnityEvent<bool> activationEvent = new();

        protected bool IsActive = false;
        protected void OnActivate()
        {
            activationEvent.Invoke(true);
            IsActive = true;
        }

        protected void OnDeactivate()
        {
            activationEvent.Invoke(false);
            IsActive = false;
        }

        public abstract bool ShouldBeEnabled(HashSet<Type> activeStates);
    }
}