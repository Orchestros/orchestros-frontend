using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    /// <summary>
    /// Provides file-related operations for a Unity project, such as loading and saving files and capturing image uploads.
    /// </summary>
    [RequireComponent(typeof(ArgosWebSync))]
    public class ArgosFileLoader : MonoBehaviour
    {
        private string _lastFetchedUrl = ""; // URL of the last fetched file
#pragma warning disable CS0414
        private bool _didFetchUrl = false; // Flag indicating whether a file has been selected by the user
#pragma warning restore CS0414

        public ArgosWebSync argosWebSync;


        /// <summary>
        /// Returns an instance of the ArgosFileLoader class from the scene's EventSystem object.
        /// </summary>
        /// <returns>An instance of the ArgosFileLoader class.</returns>
        public ArgosFileLoader GetArgosFileLoader()
        {
            return GameObject.Find("EventSystem").GetComponent<ArgosFileLoader>();
        }

        /// <summary>
        /// Saves a file to disk.
        /// </summary>
        /// <param name="filename">The name of the file to save.</param>
        /// <param name="textContent">The content of the file to save.</param>
        public void SaveFile(string filename, string textContent)
        {
            // IF web
#if !UNITY_WEBGL
            File.WriteAllText(filename, textContent);
#else
            argosWebSync.PostArgosFileToUrl(textContent);
#endif
        }

        /// <summary>
        /// Retrieves the content of a file at a given path or URL.
        /// </summary>
        /// <param name="pathOrUrl">The path or URL of the file to retrieve.</param>
        /// <returns>The content of the file as a string.</returns>
        public string GetContentFromPathOrUrl(string pathOrUrl)
        {
#if !UNITY_WEBGL
            return File.ReadAllText(pathOrUrl);
#else
            return argosWebSync.LoadArgosFileFromUrl();
#endif
        }

        /// <summary>
        /// Prompts the user to select an Argos file to load or save.
        /// </summary>
        /// <param name="newFile">A flag indicating whether to prompt the user to save a new file or load an existing file.</param>
        /// <returns>The path or URL of the selected file.</returns>
        public string GetArgosFilePathFromUser(bool newFile = false)
        {
#if !UNITY_WEBGL
            var path = "";
        
            // Using System.Windows.Forms for OpenFileDialog

            FileDialog dialog;
            
            if (newFile) {
                dialog = new SaveFileDialog();
            } else {
                dialog = new OpenFileDialog();
            }
            
            dialog.Filter = "argos,xml files (*.argos,*.xml)|*.argos;*.xml";
            dialog.Title = "Load argos file";

            var dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                path = dialog.FileName;
            }
            Debug.Log("Selected file: " + path);
            Debug.Log("Dialog result: " + dialogResult);
            

            return path;
#else
            return _lastFetchedUrl;
#endif
        }
    }
}