#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace rStarEditor
{
    /// <summary>
    ///     A base class for creating editors that decorate Unity's built-in editor types.
    /// </summary>
    public abstract class DecoratorEditor : Editor
    {
    #region Private Variables

        // empty array for invoking methods using reflection
        private static readonly object[] EMPTY_ARRAY = new object[0];

        private static readonly Dictionary<string , MethodInfo> decoratedMethods = new Dictionary<string , MethodInfo>();

        private static readonly Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));

        /// <summary>
        ///     Type object for the internally used (decorated) editor.
        /// </summary>
        private readonly Type decoratedEditorType;

        /// <summary>
        ///     Type object for the object that is edited by this editor.
        /// </summary>
        private Type editedObjectType;

        private Editor editorInstance;

        private Editor EditorInstance
        {
            get
            {
                if (editorInstance == null && targets != null && targets.Length > 0)
                    editorInstance = CreateEditor(targets , decoratedEditorType);

                if (editorInstance == null) Debug.LogError("Could not create editor !");

                return editorInstance;
            }
        }

    #endregion

    #region Constructor

        public DecoratorEditor(string editorTypeName)
        {
            decoratedEditorType = editorAssembly.GetTypes().Where(t => t.Name == editorTypeName).FirstOrDefault();

            Init();

            // Check CustomEditor types.
            var originalEditedType = GetCustomEditorType(decoratedEditorType);

            if (originalEditedType != editedObjectType)
                throw new ArgumentException(
                        string.Format("Type {0} does not match the editor {1} type {2}" ,
                                      editedObjectType , editorTypeName , originalEditedType));
        }

    #endregion

    #region Public Methods

        public override void DrawPreview(Rect previewArea)
        {
            EditorInstance.DrawPreview(previewArea);
        }

        public override string GetInfoString()
        {
            return EditorInstance.GetInfoString();
        }

        public override GUIContent GetPreviewTitle()
        {
            return EditorInstance.GetPreviewTitle();
        }

        public override bool HasPreviewGUI()
        {
            return EditorInstance.HasPreviewGUI();
        }

        public override void OnInspectorGUI()
        {
            EditorInstance.OnInspectorGUI();
        }

        public override void OnInteractivePreviewGUI(Rect r , GUIStyle background)
        {
            EditorInstance.OnInteractivePreviewGUI(r , background);
        }

        public override void OnPreviewGUI(Rect r , GUIStyle background)
        {
            EditorInstance.OnPreviewGUI(r , background);
        }

        public override void OnPreviewSettings()
        {
            EditorInstance.OnPreviewSettings();
        }

        public void OnSceneGUI()
        {
            CallInspectorMethod("OnSceneGUI");
        }

        public override void ReloadPreviewInstances()
        {
            EditorInstance.ReloadPreviewInstances();
        }

        public override Texture2D RenderStaticPreview(string assetPath , Object[] subAssets , int width , int height)
        {
            return EditorInstance.RenderStaticPreview(assetPath , subAssets , width , height);
        }

        public override bool RequiresConstantRepaint()
        {
            return EditorInstance.RequiresConstantRepaint();
        }

        public override bool UseDefaultMargins()
        {
            return EditorInstance.UseDefaultMargins();
        }

    #endregion

    #region Protected Methods

        /// <summary>
        ///     Delegates a method call with the given name to the decorated editor instance.
        /// </summary>
        protected void CallInspectorMethod(string methodName)
        {
            MethodInfo method = null;

            // Add MethodInfo to cache
            if (!decoratedMethods.ContainsKey(methodName))
            {
                var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

                method = decoratedEditorType.GetMethod(methodName , flags);

                if (method != null) decoratedMethods[methodName] = method;
                else Debug.LogError(string.Format("Could not find method {0}" , method));
            }
            else
            {
                method = decoratedMethods[methodName];
            }

            if (method != null) method.Invoke(EditorInstance , EMPTY_ARRAY);
        }

        protected override void OnHeaderGUI()
        {
            CallInspectorMethod("OnHeaderGUI");
        }

    #endregion

    #region Private Methods

        private Type GetCustomEditorType(Type type)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var attributes = type.GetCustomAttributes(typeof(CustomEditor) , true) as CustomEditor[];
            var field      = attributes.Select(editor => editor.GetType().GetField("m_InspectedType" , flags)).First();

            return field.GetValue(attributes[0]) as Type;
        }

        private void Init()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var attributes = GetType().GetCustomAttributes(typeof(CustomEditor) , true) as CustomEditor[];
            var field      = attributes.Select(editor => editor.GetType().GetField("m_InspectedType" , flags)).First();

            editedObjectType = field.GetValue(attributes[0]) as Type;
        }

        private void OnDisable()
        {
            if (editorInstance != null) DestroyImmediate(editorInstance);
        }

    #endregion
    }
}