using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FormController : MonoBehaviour
    {
        private FormConfiguration _configuration;

        public GameObject textFieldPrefab;

        private readonly Dictionary<string, TMP_InputField> _fields = new();

        public void OnSaveClicked()
        {
            _configuration.OnSave(
                _fields.ToDictionary((pair => pair.Key), pair => pair.Value.text)
            );
        }

        public void OnCancelClicked()
        {
            _configuration.OnCancel();
        }

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