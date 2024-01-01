#region

using System;
using System.Collections.Generic;
using System.Reflection;
using rStarEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#endregion

namespace rStar.Editor
{
    public class ProjectPreferencesDrawer : SettingsProvider
    {
    #region Private Variables

        private class Styles
        {
        #region Public Variables

            public static readonly GUIContent DisplayContentOnMouseHoverLabel =
                    EditorGUIUtility.TrTextContent("Display Content On Mouse Hover" , "On mouse hover asset to display the content");

            public static readonly GUIContent EscToCloseWindowLabel =
                    EditorGUIUtility.TrTextContent("Esc To Close Window" , "Press esc key to close current selected window");

            public static readonly GUIContent StringsLabel = EditorGUIUtility.TrTextContent("Strings" , "Modify Strings");

        #endregion
        }

        private SerializedObject   m_SerializedObject;
        private SerializedProperty displayContentOnMouseHover;
        private SerializedProperty escToCloseWindow;

    #endregion

    #region Constructor

        private ProjectPreferencesDrawer(string path , SettingsScope scopes , IEnumerable<string> keywords = null) : base(
                path , scopes , keywords) { }

    #endregion

    #region Public Methods

        [SettingsProvider]
        public static SettingsProvider CreateMySingletonProvider()
        {
            var provider = new ProjectPreferencesDrawer("rStarEditor/Settings" , SettingsScope.User ,
                                                        GetSearchKeywordsFromGUIContentProperties<Styles>());
            return provider;
        }

        public override void OnActivate(string searchContext , VisualElement rootElement)
        {
            ProjectSetting.instance.Load();
            m_SerializedObject         = new SerializedObject(ProjectSetting.instance);
            displayContentOnMouseHover = m_SerializedObject.FindProperty("displayContentOnMouseHover");
            escToCloseWindow           = m_SerializedObject.FindProperty("escToCloseWindow");
        }

        public override void OnGUI(string searchContext)
        {
            using (CreateSettingsWindowGUIScope())
            {
                m_SerializedObject.Update();
                EditorGUI.BeginChangeCheck();

                displayContentOnMouseHover.boolValue =
                        EditorGUILayout.Toggle(Styles.DisplayContentOnMouseHoverLabel , displayContentOnMouseHover.boolValue);
                escToCloseWindow.boolValue =
                        EditorGUILayout.Toggle(Styles.EscToCloseWindowLabel , escToCloseWindow.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    m_SerializedObject.ApplyModifiedProperties();
                    ProjectSetting.instance.SaveSettings();
                }
            }
        }

    #endregion

    #region Private Methods

        private IDisposable CreateSettingsWindowGUIScope()
        {
            var unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            var type                = unityEditorAssembly.GetType("UnityEditor.SettingsWindow+GUIScope");
            return Activator.CreateInstance(type) as IDisposable;
        }

    #endregion
    }
}