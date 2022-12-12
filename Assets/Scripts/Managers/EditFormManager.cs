using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using World.EditableItem;

namespace Managers
{
    public class EditFormManager : MonoBehaviourWithState
    {
        public SelectionManager selectionManager;

        public GameObject formPrefab;
        public GameObject canvas;
        private List<List<EditableItem>> _editableItemsList;

        private GameObject _formInstance;

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

            _editableItemsList = selectionManager.GetSelectedEditableItems()
                .Select(selectedItem => selectedItem.GetComponents<EditableItem>().ToList()).ToList();


            HashSet<string> sharedKeys = null;


            foreach (var items in _editableItemsList)
            {
                var editableValues = new Dictionary<string, string>();

                foreach (var editableItem in items) editableValues.AddRange(editableItem.GetEditableValues());

                if (sharedKeys == null)
                    sharedKeys = new HashSet<string>(editableValues.Keys);
                else
                    sharedKeys.IntersectWith(editableValues.Keys);


                foreach (var pair in editableValues)
                    if (summedEditableValues.ContainsKey(pair.Key))
                        summedEditableValues[pair.Key] = "";
                    else
                        summedEditableValues[pair.Key] = pair.Value;
            }

            if (sharedKeys == null) return;

            var formConfigurationItems = sharedKeys.Select(sharedKey =>
            {
                var label = sharedKey[..1].ToUpper() + sharedKey[1..].Replace("_", "");

                return new FormConfigurationItem(
                    label,
                    sharedKey,
                    summedEditableValues[sharedKey]
                );
            }).ToList();


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

        private void OnSave(Dictionary<string, string> values)
        {
            foreach (var item in _editableItemsList.SelectMany(items => items)) item.UpdateValues(values);

            OnCancel();
        }

        private void OnCancel()
        {
            Destroy(_formInstance);
            OnDeactivate();
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return selectionManager.GetSelectedEditableItems().Count > 0;
        }
    }
}