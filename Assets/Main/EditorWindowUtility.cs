#region

using System.Linq;
using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    public class EditorWindowUtility
    {
    #region Public Methods

        public static void FocusPropertyEditorWindow()
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            windows.FirstOrDefault(IsPropertyEditor)?.Focus();
        }

        public static bool IsPackageManagerWindow(EditorWindow focusedWindow)
        {
            return focusedWindow.GetType().ToString() == "UnityEditor.PackageManager.UI.PackageManagerWindow";
        }

        public static bool IsPropertyEditor(EditorWindow focusedWindow)
        {
            return focusedWindow.GetType().ToString() == "UnityEditor.PropertyEditor";
        }

    #endregion
    }
}