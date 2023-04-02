#region

using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

#endregion

namespace rStarEditor
{
    public static class PropertyEditorManager
    {
    #region Private Variables

        private static readonly Type         propertyEditorType = typeof(PropertyField).Assembly.GetType("UnityEditor.PropertyEditor");
        private static          EditorWindow singlePropertyEditorWindow;
        private static          Object       singleTarget;

    #endregion

    #region Public Methods

        public static bool OpenInPropertyEditor(Object asset , bool singleton = false)
        {
            if (asset is SceneAsset) return false;
            if (asset == null) return false;
            if (singleton)
            {
                if (singlePropertyEditorWindow == null)
                {
                    EditorUtility.OpenPropertyEditor(asset);
                    var propertyEditorWindow = propertyEditorType
                                              .GetField("s_LastPropertyEditor" , BindingFlags.Static | BindingFlags.NonPublic)
                                             ?.GetValue(null);
                    if (propertyEditorWindow != null)
                    {
                        var inspectedObject =
                                propertyEditorType.GetField("m_InspectedObject" , BindingFlags.Instance | BindingFlags.NonPublic);
                        var target = inspectedObject.GetValue(propertyEditorWindow);
                        Assert.IsTrue(target.Equals(asset));
                        singlePropertyEditorWindow = propertyEditorWindow as EditorWindow;
                        singleTarget               = asset;
                    }
                }
                else
                {
                    if (singleTarget == asset) return false;
                    var openPropertyEditorMethod = GetOpenPropertyEditorMethod();
                    var propertyEditorInstance =
                            openPropertyEditorMethod.Invoke(singlePropertyEditorWindow , new object[] { asset , false }) as
                                    EditorWindow;
                    singlePropertyEditorWindow.Close();
                    var position = singlePropertyEditorWindow.position;
                    propertyEditorInstance.position = new Rect(position.x , position.y , position.width , position.height);
                    propertyEditorInstance.Show();
                    singlePropertyEditorWindow = propertyEditorInstance;
                    singleTarget               = asset;
                }
            }
            else
            {
                EditorUtility.OpenPropertyEditor(asset);
            }

            return true;
        }

    #endregion

    #region Private Methods

        private static MethodInfo GetOpenPropertyEditorMethod()
        {
            MethodInfo methodInfo = null;

            foreach (var m in propertyEditorType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
            {
                var isOpenPropertyEditor = m.Name == "OpenPropertyEditor";
                if (isOpenPropertyEditor)
                    if (m.GetParameters().Length == 2)
                    {
                        methodInfo = m;
                        break;
                    }
            }

            return methodInfo;
        }

    #endregion
    }
}