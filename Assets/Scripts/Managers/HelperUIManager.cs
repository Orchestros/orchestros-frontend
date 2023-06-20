using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    [Serializable]
    public class HelperUIItem
    {
        public string description;
        public string shortcut;

        protected HelperUIItem(string description, string shortcut)
        {
            this.description = description;
            this.shortcut = shortcut;
        }

        public VisualElement CreateVisualElement()
        {
            var element = new VisualElement();
            element.AddToClassList("row");
            var label1 = new Label(description);
            label1.AddToClassList("description");
            element.Add(label1);
            var label2 = new Label(shortcut);
            label2.AddToClassList("shortcut");
            element.Add(label2);
            return element;
        }

    }

    public class HelperUIManager : MonoBehaviourWithState
    {
        // List of helper items, serialized so that they can be set in the Unity Editor
        [SerializeField] public HelperUIItem[] helperItems;


        // A reference to the HelperUI gameObject
        public GameObject helperUI;

        /// Hide the HelperUI gameObject on start
        private void Start()
        {

            helperUI.SetActive(false);
        }

        private void SetupView()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            var uiDocument = helperUI.GetComponent<UIDocument>();
            var rootVisualElement = uiDocument.rootVisualElement;
            var scrollView = rootVisualElement.Children().First().Children().First().Children().Last() as ScrollView;
            
            // Find button with name close and add a click event to it, so that it closes the helper UI
            var closeButton = rootVisualElement.Q<Button>("close");
            closeButton.clickable.clicked += () => helperUI.SetActive(false);

            foreach (var helperItem in helperItems)
            {
                scrollView?.contentContainer
                    .Add(helperItem.CreateVisualElement());
            }
        }

        /// Toggle UIDocument (gameObject) on/off when the user presses the 'H' key
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.H)) return;
            
            helperUI.SetActive(!helperUI.activeSelf);
            
            if (helperUI.activeSelf)
            {
                SetupView();
            }
        }
        
        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            return !activeStates.Contains(typeof(EditFormManager));
        }
    }
}