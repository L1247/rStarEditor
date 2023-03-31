#region

using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    [InitializeOnLoad]
    public class rStarEditor : Editor
    {
    #region Constructor

        static rStarEditor()
        {
            ProjectSetting.instance.Load();
            var info = typeof(EditorApplication).GetField(
                    "globalEventHandler" , BindingFlags.Static | BindingFlags.NonPublic);
            var value = (EditorApplication.CallbackFunction)info.GetValue(null);
            value += EditorGlobalKeyPress;
            info.SetValue(null , value);
        }

    #endregion

    #region Private Methods

        private static void EditorGlobalKeyPress()
        {
            // Debug.Log("KEY CHANGE " + Event.current.keyCode);
            HandleFocusedPropertyWindow();
        }

        private static void HandleFocusedPropertyWindow()
        {
            var focusedWindow = EditorWindow.focusedWindow;
            if (focusedWindow == null) return;
            var isPropertyEditor = IsPropertyEditor(focusedWindow);
            var escToCloseWindow = ProjectSetting.instance.EscToCloseWindow;
            var escPressed       = Event.current.keyCode == KeyCode.Escape && Event.current.type == EventType.KeyDown;
            if (isPropertyEditor && escPressed && escToCloseWindow)
            {
                focusedWindow.Close();
                var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
                windows.FirstOrDefault(IsPropertyEditor)?.Focus();
            }
        }

        private static bool IsPropertyEditor(EditorWindow focusedWindow)
        {
            var isPropertyEditor = focusedWindow.GetType().ToString() == "UnityEditor.PropertyEditor";
            return isPropertyEditor;
        }

    #endregion
    }
}