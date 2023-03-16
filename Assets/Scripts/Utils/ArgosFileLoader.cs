using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Utils
{
    public static class ArgosFileLoader
    {
        public static string GetArgosFilePathFromUser()
        {
            string[] extensions = { "Argos map", "argos,xml", "All files", "*" };
            // Check if is web platform
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return "";
            }

            return "";
        }
    }
}