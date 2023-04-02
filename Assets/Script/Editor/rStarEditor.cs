#region

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

#endregion

namespace rStarEditor
{
    [InitializeOnLoad]
    public class rStarEditor : Editor
    {
    #region Public Variables

        public static readonly Type type = typeof(PropertyField).Assembly.GetType("UnityEditor.PropertyEditor");

    #endregion

    #region Constructor

        static rStarEditor()
        {
            ProjectSetting.instance.Load();
            RegisterKeyPress();
            finishedDefaultHeaderGUI += EditorOnfinishedDefaultHeaderGUI;
        }

    #endregion

    #region Private Methods

        private static void EditorGlobalKeyPress()
        {
            // Debug.Log("KEY CHANGE " + Event.current.keyCode);
            HandleFocusedPropertyWindow();
        }

        private static void EditorOnfinishedDefaultHeaderGUI(Editor editor)
        {
            var target = editor.target;
            if (target != null)
                if (GUILayout.Button("Ping" , EditorStyles.miniButton))
                        // Debug.Log($"Ping");
                    Selection.activeObject = target;
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

        private static void RegisterKeyPress()
        {
            var info = typeof(EditorApplication).GetField(
                    "globalEventHandler" , BindingFlags.Static | BindingFlags.NonPublic);
            var value = (EditorApplication.CallbackFunction)info.GetValue(null);
            value += EditorGlobalKeyPress;
            info.SetValue(null , value);
        }

    #endregion
    }
}