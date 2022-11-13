using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using World.Arena.EditableItem;

namespace Managers
{
    public class EditFormManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;

        public GameObject formPrefab;
        public GameObject canvas;

        private GameObject _formInstance;
        private List<EditableItem> _editableItemsList;

        public void OpenFormForSelection()
        {
            if (!enabled || IsActive) return;
            
            OnActivate();

            BuildForm();
        }

        private void BuildForm()
        {
            _formInstance = Instantiate(formPrefab, canvas.transform);
            var formController = _formInstance.GetComponent<FormController>();

            var summedEditableValues = new Dictionary<string, string>();


            _editableItemsList = selectionManager.GetSelectedItems()
                .Select(selectedItem => selectedItem.GetComponent<EditableItem>()).ToList();

            HashSet<string> sharedKeys = null;


            foreach (var item in _editableItemsList)
            {
                var editableValues = item.GetEditableValues();

                if (sharedKeys == null)
                {
                    sharedKeys = new HashSet<string>(editableValues.Keys);
                }
                else
                {
                    sharedKeys.IntersectWith(editableValues.Keys);
                }


                foreach (var pair in editableValues)
                {
                    if (summedEditableValues.ContainsKey(pair.Key))
                    {
                        summedEditableValues[pair.Key] = "";
                    }
                    else
                    {
                        summedEditableValues[pair.Key] = pair.Value;
                    }
                }
            }

            if (sharedKeys == null) return;

            var formConfigurationItems = sharedKeys.Select(sharedKey => new FormConfigurationItem(
                    sharedKey,
                    sharedKey,
                    summedEditableValues[sharedKey]
                )
            ).ToList();


            formController.SetFormConfiguration(
                new FormConfiguration(
                    "Edit",
                    formConfigurationItems,
                    OnSave,
                    OnCancel
                )
            );
        }

        private void OnSave(Dictionary<string, string> values)
        {
            foreach (var item in _editableItemsList)
            {
                item.UpdateValues(values);
            }
            
            OnCancel();
        }

        private void OnCancel()
        {
            Destroy(_formInstance);
            OnDeactivate();
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedItems().Count > 0;
        }
    }
}