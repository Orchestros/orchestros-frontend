using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SceneManager : MonoBehaviour
    {
        private readonly HashSet<Type> _activeStates = new();
        private MonoBehaviourWithState[] _states;

        private void Start()
        {
            _states = Resources.FindObjectsOfTypeAll<MonoBehaviourWithState>();

            foreach (var state in _states)
                state.activationEvent.AddListener(isActive => OnStateActivationChanged(state, isActive));
        }

        private void OnStateActivationChanged(MonoBehaviourWithState state, bool isActive)
        {
            if (isActive)
                _activeStates.Add(state.GetType());
            else
                _activeStates.Remove(state.GetType());


            foreach (var monoBehaviourWithState in _states)
                monoBehaviourWithState.enabled = monoBehaviourWithState.ShouldBeEnabled(_activeStates);
        }
    }
}