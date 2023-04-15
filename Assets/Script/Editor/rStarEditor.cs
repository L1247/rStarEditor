#region

using System.Reflection;
using rStar.Editor;
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
                    // GUI.backgroundColor = new Color(0.02f , 0.98f , 1f);
                using (new GUILayout.HorizontalScope())
                {
                    if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.Height(50)) , "Select"))
                        EditorGUIUtility.PingObject(target);
                    PopUpAssetInspector.ExampleDragDropGUI(EditorGUILayout.GetControlRect(GUILayout.Height(50)) , target);
                }
        }

        private static void HandleFocusedPropertyWindow()
        {
            var focusedWindow = EditorWindow.focusedWindow;
            if (focusedWindow == null) return;
            var isPropertyEditor = EditorWindowUtility.IsPropertyEditor(focusedWindow);
            var escToCloseWindow = ProjectSetting.instance.EscToCloseWindow;
            var escPressed       = Event.current.keyCode == KeyCode.Escape && Event.current.type == EventType.KeyDown;
            if (isPropertyEditor && escPressed && escToCloseWindow)
            {
                focusedWindow.Close();
                EditorWindowUtility.FocusPropertyEditorWindow();
            }
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