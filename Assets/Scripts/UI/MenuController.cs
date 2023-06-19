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
    public class MenuController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Button _newSceneButton;
        private Button _openSceneButton;
        private Button _loadDemoButton;
        private ScrollView _historyScrollView;
        public ArgosFileLoader argosFileLoader;

        /// <summary>
        /// Sets up event handlers for the UI buttons.
        /// </summary>
        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();

            _historyScrollView = _uiDocument.rootVisualElement.Query<ScrollView>("historyScrollView").First();
            var isHistoryEmpty = true;
            for (var i = 0; i < 10; i++)
            {
                
                var historyItem = PlayerPrefs.GetString("history_" + i);
                if (historyItem.Length <= 0) continue;

                isHistoryEmpty = false;
                var button = new Button(() =>
                {
                    GlobalVariables.Set(GlobalVariablesKey.ArgosFile, historyItem);
                    SceneManager.LoadScene("ArgosMapEditor");
                })
                {
                    // Text with backslashes is interpreted as a path, so we need to escape them
                    text = historyItem.Replace("\\", "\\\\")
                };

                button.AddToClassList("history-item");
                _historyScrollView.contentContainer.Add(button);
            }
            
            if (isHistoryEmpty)
            {
                _historyScrollView.contentContainer.Add(new Label("No history yet"));
            }
            
            
            
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