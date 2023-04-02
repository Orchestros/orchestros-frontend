using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    /// <summary>
    /// A base class for mono behaviours that have an active state that can be toggled on and off.
    /// </summary>
    public abstract class MonoBehaviourWithState : MonoBehaviour
    {
        [SerializeField]
        public UnityEvent<bool>
            activationEvent = new(); // an event that is called when the state of the manager is changed

        protected bool IsActive; // whether the manager is currently active

        /// <summary>
        /// Called when the manager is activated.
        /// </summary>
        protected void OnActivate()
        {
            activationEvent.Invoke(true);
            IsActive = true;
        }

        /// <summary>
        /// Called when the manager is deactivated.
        /// </summary>
        protected void OnDeactivate()
        {
            // If the manager is not active, do nothing
            if (!IsActive)
            {
                return;
            }

            activationEvent.Invoke(false);
            IsActive = false;
        }

        /// <summary>
        /// Determines whether the manager should be enabled based on the set of currently active states.
        /// </summary>
        /// <param name="activeStates">The set of currently active states.</param>
        /// <returns>True if the manager should be enabled, false otherwise.</returns>
        public abstract bool ShouldBeEnabled(HashSet<Type> activeStates);
    }
}