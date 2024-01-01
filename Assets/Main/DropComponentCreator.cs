#region

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace rStarEditor
{
    [InitializeOnLoad]
    public class DropComponentCreator : Editor
    {
    #region Private Variables

        private static Object[] selectedGameObjects;

    #endregion

    #region Constructor

        static DropComponentCreator()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
            EditorApplication.hierarchyWindowChanged   += HierarchyWindowChanged;
        }

    #endregion

    #region Private Methods

        private static Type GetType(string typeName)
        {
            Type result = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (!type.Name.Equals(typeName)) continue;
                    result = type;
                    break;
                }
            }

            return result;
        }

        private static void HierarchyWindowChanged()
        {
            if (selectedGameObjects != null && selectedGameObjects.Length > 0) Selection.objects = selectedGameObjects;
        }

        private static void HierarchyWindowItemCallback(int pID , Rect pRect)
        {
            var eventType = Event.current.type;
            if (eventType != EventType.DragExited) return;
            var selectedObjects = new List<GameObject>();
            foreach (var objectRef in DragAndDrop.objectReferences)
            {
                var isEditorScript = objectRef.GetType() == typeof(MonoScript);
                var isTexture2D    = objectRef is Texture2D;
                if (isEditorScript == false && isTexture2D == false) break;
                var scriptName = objectRef.name;
                var dropType   = GetType(scriptName);
                if (dropType is null) return;
                var isMonoBehaviour = dropType.BaseType != typeof(MonoBehaviour);
                if (isMonoBehaviour) continue;
                var gameObject = new GameObject(scriptName);
                gameObject.AddComponent(dropType);
                selectedObjects.Add(gameObject);
            }

            if (selectedObjects.Count == 0) return;
            selectedGameObjects = selectedObjects.ToArray();
            Event.current.Use();
        }

    #endregion
    }
}