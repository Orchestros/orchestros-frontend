using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// A scene manager that keeps track of active states and enables/disables MonoBehaviours based on those states.
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        // A hash set that stores the currently active states.
        private readonly HashSet<Type> _activeStates = new();

        // An array of MonoBehaviours with state.
        private MonoBehaviourWithState[] _states;

        // Called once when the script is first enabled.
        private void Start()
        {
            // Find all MonoBehaviours with state in the resources folder.
            _states = Resources.FindObjectsOfTypeAll<MonoBehaviourWithState>();

            // Add a listener to the activation event of each state.
            foreach (var state in _states)
            {
                state.activationEvent.AddListener(isActive => OnStateActivationChanged(state, isActive));
            }
        }

        // Called once per frame.
        private void Update()
        {
            // If the left shift key and escape key are pressed down, load the menu scene.
            if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Menu");
            }
        }

        // Called when the activation state of a MonoBehaviourWithState changes.
        private void OnStateActivationChanged(MonoBehaviourWithState state, bool isActive)
        {
            if (isActive)
            {
                // If the state is active, add its type to the hash set of active states.
                _activeStates.Add(state.GetType());
            }
            else
            {
                // If the state is inactive, remove its type from the hash set of active states.
                _activeStates.Remove(state.GetType());
            }

            // Enable or disable each MonoBehaviourWithState based on the hash set of active states.
            foreach (var monoBehaviourWithState in _states)
            {
                monoBehaviourWithState.enabled = monoBehaviourWithState.ShouldBeEnabled(_activeStates);
            }
        }
    }
}