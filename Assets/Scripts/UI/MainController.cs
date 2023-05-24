using Managers;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace UI
{
    /// <summary>
    /// Controller for the main menu UI.
    /// </summary>
    public class MainController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Button _newSceneButton;
        private Button _openSceneButton;
        private Button _loadDemoButton;
        public ArgosFileLoader argosFileLoader;

        /// <summary>
        /// Sets up event handlers for the UI buttons.
        /// </summary>
        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            _newSceneButton = _uiDocument.rootVisualElement.Query<Button>("newMap").First();
            _newSceneButton.clickable.clicked += () =>
            {
                // Reset ArgosFile
                GlobalVariables.Set(GlobalVariablesKey.ArgosFile, "");
                SceneManager.LoadScene("Scenes/ArgosMapEditor");
            };

            _openSceneButton = _uiDocument.rootVisualElement.Query<Button>("openMap").First();
            _openSceneButton.clickable.clicked += () =>
            {
                argosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser(
                    file =>
                    {
                        if (file.Length <= 0) return;

                        SceneManager.UnloadSceneAsync("Menu");
                        SceneManager.LoadScene("ArgosMapEditor");
                        GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
                    }
                );
            };

            _loadDemoButton = _uiDocument.rootVisualElement.Query<Button>("newDemo").First();
            _loadDemoButton.clickable.clicked += () =>
            {
                argosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser(
                    file =>
                    {
                        if (file.Length <= 0) return;

                        SceneManager.UnloadSceneAsync("Menu");
                        SceneManager.LoadScene("Scenes/Demonstrator");
                        GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
                    });
            };
        }
    }
}