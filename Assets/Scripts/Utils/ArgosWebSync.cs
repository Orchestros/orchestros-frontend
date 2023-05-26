using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
    public class ArgosWebSync : MonoBehaviour
    {
        private static string _saveUrl;

        private void Start()
        {
# if UNITY_WEBGL
            LoadArgosFileFromUrl();
            GlobalVariables.Set(GlobalVariablesKey.ArgosFile, "web");
#endif
        }

        public static string LoadArgosFileFromUrl()
        {
#if !UNITY_WEBGL
            return "";
#endif

            var absoluteURL = Application.absoluteURL;
            var queryParameters = GetQueryParameters(absoluteURL);

            var decodedData = "";
            var decodedUrl = "";

            if (queryParameters.TryGetValue("data", out var queryParameter))
            {
                // Decode using base 64
                decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(queryParameter));
            }

            if (queryParameters.TryGetValue("output_to", out var parameter))
            {
                // Decode using base 64
                decodedUrl = Encoding.UTF8.GetString(Convert.FromBase64String(parameter));
            }

            _saveUrl = decodedUrl;

            return decodedData;
        }

        public void PostArgosFileToUrl(string data)
        {
# if !UNITY_WEBGL
            return;
# endif
            // Make web request to URL with file data as the body
            var request = new UnityWebRequest(_saveUrl, "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(data);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

            // Set headers
            request.SetRequestHeader("Content-Type", "application/xml");

            // Send request
            request.SendWebRequest();
        }

        private static Dictionary<string, string> GetQueryParameters(string url)
        {
            var parameters = new Dictionary<string, string>();

            var questionMarkIndex = url.IndexOf('?');
            
            if (questionMarkIndex < 0 || questionMarkIndex >= url.Length - 1) return parameters;
            
            var queryString = url[(questionMarkIndex + 1)..];
            var queryPairs = queryString.Split('&');

            foreach (var pair in queryPairs)
            {
                var keyValue = pair.Split('=', 2);
                if (keyValue.Length != 2) continue;
                var key = Uri.UnescapeDataString(keyValue[0]);
                var value = Uri.UnescapeDataString(keyValue[1]);
                parameters[key] = value;
            }

            return parameters;
        }
    }
}
