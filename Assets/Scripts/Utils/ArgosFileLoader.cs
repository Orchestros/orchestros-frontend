using UnityEditor;

namespace Utils
{
    public static class ArgosFileLoader
    {
        public static string GetArgosFilePathFromUser()
        {
            string[] extensions = { "Argos map", "argos,xml", "All files", "*" };
            var file = EditorUtility.OpenFilePanelWithFilters("Select Argos File", "", extensions);
            return file;
        }
    }
}