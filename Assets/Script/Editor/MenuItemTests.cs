#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Script.Editor
{
    public class MenuItem : EditorWindow
    {
    #region Private Methods

        [UnityEditor.MenuItem("Tools/Test _h")]
        private static void ShowWindow()
        {
            var objectsOfTypeAll = Resources.FindObjectsOfTypeAll<EditorWindow>();
            Debug.Log($"{objectsOfTypeAll.Length}");
            Debug.Log($"{focusedWindow.GetType()}");
        }

    #endregion
    }
}