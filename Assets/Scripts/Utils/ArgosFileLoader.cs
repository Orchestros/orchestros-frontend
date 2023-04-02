using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Networking;

namespace Utils
{
    /// <summary>
    /// Provides file-related operations for a Unity project, such as loading and saving files and capturing image uploads.
    /// </summary>
    public class ArgosFileLoader : MonoBehaviour
    {
        private string _lastFetchedUrl = ""; // URL of the last fetched file
#pragma warning disable CS0414
        private bool _didFetchUrl = false; // Flag indicating whether a file has been selected by the user
#pragma warning restore CS0414

        [DllImport("__Internal")]
        public static extern void BrowserTextDownload(string filename, string textContent);

        /// <summary>
        /// Initializes a UniTask loop when the game is launched.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitUniTaskLoop()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref loop);
        }

        /// <summary>
        /// Returns an instance of the ArgosFileLoader class from the scene's EventSystem object.
        /// </summary>
        /// <returns>An instance of the ArgosFileLoader class.</returns>
        public static ArgosFileLoader GetArgosFileLoader()
        {
            return GameObject.Find("EventSystem").GetComponent<ArgosFileLoader>();
        }

        /// <summary>
        /// Saves a file to disk.
        /// </summary>
        /// <param name="filename">The name of the file to save.</param>
        /// <param name="textContent">The content of the file to save.</param>
        public static void SaveFile(string filename, string textContent)
        {
#if UNITY_EDITOR
            System.IO.File.WriteAllText(filename, textContent);
#else
            BrowserTextDownload(filename, textContent);
#endif
        }

        /// <summary>
        /// Retrieves the content of a file at a given path or URL.
        /// </summary>
        /// <param name="pathOrUrl">The path or URL of the file to retrieve.</param>
        /// <returns>The content of the file as a string.</returns>
        public async Task<string> GetContentFromPathOrUrl(string pathOrUrl)
        {
#if UNITY_EDITOR
            return System.IO.File.ReadAllText(pathOrUrl);
#else
            UnityWebRequest www = UnityWebRequest.Get(pathOrUrl);
            www.SendWebRequest();
            
            // Wait for the request to complete
            await UniTask.Delay(300);

            return www.downloadHandler.text;
#endif
        }

        /// <summary>
        /// Prompts the user to select an Argos file to load or save.
        /// </summary>
        /// <param name="newFile">A flag indicating whether to prompt the user to save a new file or load an existing file.</param>
        /// <returns>The path or URL of the selected file.</returns>
        public async Task<string> GetArgosFilePathFromUser(bool newFile = false)
        {
#if UNITY_EDITOR
            var path = newFile
                ? EditorUtility.SaveFilePanel("Save argos file", "", "map2.argos", "argos,xml")
                : EditorUtility.OpenFilePanel("Load argos file", "", "argos,xml");
            return path;
#else
            ImageUploaderCaptureClick();
            while (!didFetchUrl)
            {
                Debug.Log("Waiting for file to be selected");
                // Wait for the file to be selected
                await UniTask.Delay(300);
            }
            
        _didFetchUrl = false;
        
        return _lastFetchedUrl;
#endif
        }

        [DllImport("__Internal")]
        private static extern void ImageUploaderCaptureClick();

        /// <summary>
        /// Called when a file is selected by the user. Sets the didFetchUrl flag to true and saves the URL of the selected file to lastFetchedUrl.
        /// </summary>
        /// <param name="url">The URL of the selected file.</param>
        private void FileSelected(string url)
        {
            _didFetchUrl = true;
            _lastFetchedUrl = url;
        }
    }
}