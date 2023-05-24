using System;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Provides file-related operations for a Unity project, such as loading and saving files and capturing image uploads.
    /// </summary>
    [RequireComponent(typeof(ArgosWebSync))]
    public class ArgosFileLoader : MonoBehaviour
    {
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
        /// <param name="onComplete">The action to call once the file dialog has been closed.</param>
        /// <param name="newFile">A flag indicating whether to prompt the user to save a new file or load an existing file.</param>
        /// <returns>The path or URL of the selected file.</returns>
        public void GetArgosFilePathFromUser(Action<string> onComplete, bool newFile = false)
        {
#if !UNITY_WEBGL
            // Using System.Windows.Forms for OpenFileDialog
            FileBrowser.SetFilters(true, new FileBrowser.Filter("argos,xml files", ".argos", ".xml"));
            FileBrowser.SetDefaultFilter(".argos");

            if (newFile)
            {
                FileBrowser.ShowSaveDialog(
                    onSuccess: (e) => onComplete(e[0]),
                    onCancel: () => onComplete(""),
                    pickMode: FileBrowser.PickMode.Files,
                    initialFilename:"untitled.argos",
                    saveButtonText: "Save Arena",
                    allowMultiSelection: false
                );
            }
            else
            {
                FileBrowser.ShowLoadDialog(
                    onSuccess: (e) => onComplete(e[0]),
                    onCancel: () => onComplete(""),
                    loadButtonText: "Load Arena",
                    pickMode: FileBrowser.PickMode.Files,
                    allowMultiSelection: false
                );
            }

#else
            onComplete("");
#endif
        }
    }
}