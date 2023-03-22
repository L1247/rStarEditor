#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#endregion

namespace rStar.Editor
{
    public class ProjectPreferences : SettingsProvider
    {
    #region Private Variables

        private class Styles
        {
        #region Public Variables

            public static readonly GUIContent NumberLabel =
                    EditorGUIUtility.TrTextContent("Display Content On Mouse Hover" , "Modify Number");

            public static readonly GUIContent StringsLabel = EditorGUIUtility.TrTextContent("Strings" , "Modify Strings");

        #endregion
        }

        private SerializedObject   m_SerializedObject;
        private SerializedProperty displayContentOnMouseHover;

    #endregion

    #region Constructor

        public ProjectPreferences(string path , SettingsScope scopes , IEnumerable<string> keywords = null) : base(
                path , scopes , keywords) { }

    #endregion

    #region Public Methods

        [SettingsProvider]
        public static SettingsProvider CreateMySingletonProvider()
        {
            var provider = new ProjectPreferences("rStarEditor/ProjectSetting" , SettingsScope.User ,
                                                  GetSearchKeywordsFromGUIContentProperties<Styles>());
            return provider;
        }

        public override void OnActivate(string searchContext , VisualElement rootElement)
        {
            m_SerializedObject         = new SerializedObject(ProjectSetting.instance);
            displayContentOnMouseHover = m_SerializedObject.FindProperty("displayContentOnMouseHover");
        }

        public override void OnGUI(string searchContext)
        {
            using (CreateSettingsWindowGUIScope())
            {
                m_SerializedObject.Update();
                EditorGUI.BeginChangeCheck();

                displayContentOnMouseHover.boolValue =
                        EditorGUILayout.Toggle(Styles.NumberLabel , displayContentOnMouseHover.boolValue);

                if (EditorGUI.EndChangeCheck()) m_SerializedObject.ApplyModifiedProperties();
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