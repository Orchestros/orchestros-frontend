using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using World.EditableItem;

namespace Managers
{
    /// <summary>
    /// Manages the edit form for selected editable items.
    /// </summary>
    public class EditFormManager : MonoBehaviourWithState
    {
        [SerializeField] private SelectionManager selectionManager; // the selection manager
        [SerializeField] private GameObject formPrefab; // the prefab for the edit form
        [SerializeField] private GameObject canvas; // the canvas to use for displaying the edit form
        private List<List<EditableItem>> _editableItemsList; // the list of selected editable items

        private GameObject _formInstance; // the instance of the edit form

        /// <summary>
        /// Opens the edit form for the currently selected items.
        /// </summary>
        public void OpenFormForSelection()
        {
            // If the manager is disabled or already active, do nothing
            if (!enabled || IsActive)
            {
                return;
            }

            OnActivate();

            BuildForm();
        }

        /// <summary>
        /// Builds the edit form.
        /// </summary>
        private void BuildForm()
        {
            _formInstance = Instantiate(formPrefab, canvas.transform);
            var formController = _formInstance.GetComponent<FormController>();

            var summedEditableValues = new Dictionary<string, string>();

            // Get the selected editable items and their editable values
            _editableItemsList = selectionManager.GetSelectedEditableItems()
                .Select(selectedItem => selectedItem.GetComponents<EditableItem>().ToList()).ToList();

            HashSet<string> sharedKeys = null;

            foreach (var items in _editableItemsList)
            {
                var editableValues = new Dictionary<string, string>();

                foreach (var editableItem in items)
                {
                    editableValues.AddRange(editableItem.GetEditableValues());
                }

                // Find the keys that are shared across all the selected items
                if (sharedKeys == null)
                {
                    sharedKeys = new HashSet<string>(editableValues.Keys);
                }
                else
                {
                    sharedKeys.IntersectWith(editableValues.Keys);
                }

                // Sum up the values for each key
                foreach (var pair in editableValues)
                {
                    if (summedEditableValues.ContainsKey(pair.Key) && summedEditableValues[pair.Key] != pair.Value)
                    {
                        summedEditableValues[pair.Key] = "";
                    }
                    else
                    {
                        summedEditableValues[pair.Key] = pair.Value;
                    }
                }
            }

            // If no shared keys, do nothing
            if (sharedKeys == null)
            {
                return;
            }

            // Create form configuration items for each shared key
            var formConfigurationItems = sharedKeys.Select(sharedKey =>
            {
                var label = sharedKey[..1].ToUpper() + sharedKey[1..].Replace("_", "");

                return new FormConfigurationItem(
                    label,
                    sharedKey,
                    summedEditableValues[sharedKey]
                );
            }).ToList();

            // Set the form configuration
            formController.SetFormConfiguration(
                new FormConfiguration(
                    "Edit",
                    formConfigurationItems,
                    OnSave,
                    OnCancel
                )
            );

            _formInstance.transform.SetLocalPositionAndRotation(
                new Vector3(290, 0, 0), Quaternion.identity);
        }

        /// <summary>
        /// Called when the edit form is saved.
        /// </summary>
        /// <param name="values">The edited values.</param>
        private void OnSave(Dictionary<string, string> values)
        {
            // Remove values with empty strings
            values = values.Where(pair => !string.IsNullOrEmpty(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Update the selected editable items with the edited values
            foreach (var item in _editableItemsList.SelectMany(items => items))
            {
                item.UpdateValues(values);
            }

            // Close the edit form
            OnCancel();
        }

        /// <summary>
        /// Called when the edit form is cancelled.
        /// </summary>
        private void OnCancel()
        {
            Destroy(_formInstance);
            OnDeactivate();
        }

        /// <inheritdoc />
        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            // The manager should be enabled if there are selected editable items
            return selectionManager.GetSelectedEditableItems().Count > 0;
        }
    }
}