using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Controller for a form UI.
    /// </summary>
    public class FormController : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the input fields in the form.
        /// </summary>
        public GameObject textFieldPrefab;

        private readonly Dictionary<string, TMP_InputField> _fields = new();
        private FormConfiguration _configuration;

        /// <summary>
        /// Handles a click on the save button.
        /// </summary>
        public void OnSaveClicked()
        {
            _configuration.OnSave(
                _fields.ToDictionary(pair => pair.Key, pair => pair.Value.text)
            );
        }

        /// <summary>
        /// Handles a click on the cancel button.
        /// </summary>
        public void OnCancelClicked()
        {
            _configuration.OnCancel();
        }

        /// <summary>
        /// Sets the form configuration for the UI.
        /// </summary>
        /// <param name="configuration">The form configuration to set.</param>
        public void SetFormConfiguration(FormConfiguration configuration)
        {
            _configuration = configuration;

            transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = _configuration.Title;

            var inputsZone = transform.GetChild(1);

            foreach (var item in configuration.Items)
            {
                var inputField = Instantiate(textFieldPrefab, inputsZone, false);
                inputField.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Label;
                var tmpInputField = inputField.GetComponentInChildren<TMP_InputField>();
                tmpInputField.text = item.DefaultValue;
                _fields[item.ID] = tmpInputField;
            }

            transform.localPosition = new Vector3(289, 84);
        }
    }
}