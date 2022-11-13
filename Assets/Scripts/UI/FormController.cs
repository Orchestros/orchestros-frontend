using System;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FormController : MonoBehaviour
    {
        private FormConfiguration _configuration;

        public GameObject titlePrefab;
        public GameObject textFieldPrefab;
        public GameObject closeButtonPrefab;
        public GameObject saveButtonPrefab;


        void SetFormConfiguration(FormConfiguration configuration)
        {
            _configuration = configuration;

            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            var g = Instantiate(titlePrefab, Vector3.zero, Quaternion.identity);
            g.GetComponentInChildren<TextMeshProUGUI>().text = _configuration.Title;
            g.transform.parent = transform;
        }
    }
}