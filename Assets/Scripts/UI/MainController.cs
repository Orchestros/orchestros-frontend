using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
    public class MainController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Button _newSceneButton;
        private Button _openSceneButton;
        private Button _loadDemoButton;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            _newSceneButton = _uiDocument.rootVisualElement.Query<Button>("newMap").First();
            _newSceneButton.clickable.clicked += () => { SceneManager.LoadScene("Scenes/ArgosMapEditor"); };

            _openSceneButton = _uiDocument.rootVisualElement.Query<Button>("openMap").First();
            _openSceneButton.clickable.clicked += async () =>
            {
                var file = await ArgosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser();

                if (file.Length <= 0) return;

                SceneManager.UnloadSceneAsync("Menu");
                SceneManager.LoadScene("ArgosMapEditor");
                GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
            };

            _loadDemoButton = _uiDocument.rootVisualElement.Query<Button>("newDemo").First();
            _loadDemoButton.clickable.clicked += async () =>
            {
                var file = await ArgosFileLoader.GetArgosFileLoader().GetArgosFilePathFromUser();

                if (file.Length <= 0) return;
                SceneManager.UnloadSceneAsync("Menu");
                SceneManager.LoadScene("Scenes/Demonstrator");
                GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
            };
        }
    }
}