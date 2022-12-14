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

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            _newSceneButton = _uiDocument.rootVisualElement.Query<Button>("newMap").First();
            _newSceneButton.clickable.clicked += () =>
            {
                SceneManager.LoadScene("Scenes/ArgosMapEditor");
            };

            _newSceneButton = _uiDocument.rootVisualElement.Query<Button>("openMap").First();
            _newSceneButton.clickable.clicked += () =>
            {
                SceneManager.UnloadSceneAsync("Menu");
                SceneManager.LoadScene("ArgosMapEditor");

                var file = ArgosFileLoader.GetArgosFilePathFromUser();
                
                if (file.Length > 0)
                {
                    GlobalVariables.Set(GlobalVariablesKey.ArgosFile, file);
                }
            };
        }
    }
}