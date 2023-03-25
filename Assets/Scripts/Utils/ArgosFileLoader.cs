using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Networking;

namespace Utils
{
    public class ArgosFileLoader : MonoBehaviour
    {
        private string _lastFetchedUrl = "";
#pragma warning disable CS0414
        private bool _didFetchUrl = false;
#pragma warning restore CS0414


        [DllImport("__Internal")]
        public static extern void BrowserTextDownload(string filename, string textContent);
        
        
        
        // AfterAssembliesLoaded is called before BeforeSceneLoad
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitUniTaskLoop()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref loop);
        }
        
        [DllImport("__Internal")]
        private static extern void ImageUploaderCaptureClick();

        public static ArgosFileLoader GetArgosFileLoader()
        {
            return GameObject.Find("EventSystem").GetComponent<ArgosFileLoader>();
        }


        public static void SaveFile(string filename, string textContent)
        {
            #if UNITY_EDITOR
            System.IO.File.WriteAllText(filename, textContent);
            #else
            BrowserTextDownload(filename, textContent);
            #endif
        }
        
#pragma warning disable CS1998
        public async Task<string> GetContentFromPathOrUrl(string pathOrUrl)
#pragma warning restore CS1998
        {
#if UNITY_EDITOR
            return System.IO.File.ReadAllText(pathOrUrl);
#else
            UnityWebRequest www = UnityWebRequest.Get(pathOrUrl);
            www.SendWebRequest();
            
                await UniTask.Delay(300);


            return www.downloadHandler.text;
#endif
        }

        public async Task<string> GetArgosFilePathFromUser()
        {
            string[] extensions = { "Argos map", "argos,xml", "All files", "*" };
#if UNITY_EDITOR
            var path = EditorUtility.OpenFilePanel("Open image", "", "jpg,png,bmp");
            return path;
#else
            ImageUploaderCaptureClick();
            while (!_didFetchUrl)
            {
                Debug.Log("Waiting for file to be selected");
                // Wait for the file to be selected
                await UniTask.Delay(300);
            }
            
            _didFetchUrl = false;
            
            return _lastFetchedUrl;
#endif
        }

        private void FileSelected(string url)
        {
#if !UNITY_EDITOR
            Debug.Log("File selected: " + url);
#endif
            _didFetchUrl = true;
            _lastFetchedUrl = url;
        }
    }
}