#region

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#endregion

namespace Script.Editor
{
    // makes sure that the static constructor is always called in the editor.
    [InitializeOnLoad]
    public class ComponentXCreator : UnityEditor.Editor
    {
    #region Private Variables

        private static GameObject[] selectedGameObjects;

    #endregion

    #region Constructor

        static ComponentXCreator()
        {
            // Adds a callback for when the hierarchy window processes GUI events
            // for every GameObject in the heirarchy.
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
            // happens when an acceptable item is released over the GUI window
            // Debug.Log($"{eventType}");
            if (eventType == EventType.DragExited)
            {
                // Debug.Log($"exit {DragAndDrop.objectReferences.Length} , {Selection.count}");

                // get all the drag and drop information ready for processing.
                // DragAndDrop.AcceptDrag();
                // used to emulate selection of new objects.
                var selectedObjects = new List<GameObject>();
                // run through each object that was dragged in.
                for (var index = 0 ; index < DragAndDrop.objectReferences.Length ; index++)
                {
                    var objectRef      = DragAndDrop.objectReferences[index];
                    var isEditorScript = objectRef.GetType() == typeof(MonoScript);
                    var isTexture2D    = objectRef is Texture2D;
                    // Debug.Log($"{objectRef}");
                    if (isEditorScript == false && isTexture2D == false) break;

                    var scriptName = objectRef.name;
                    var type       = GetType(scriptName);
                    if (isTexture2D)
                    {
                        Event.current.Use();
                        if (Selection.count > 1)
                        {
                            // hold alt when dragging
                            // var gameObject     = new GameObject(objectRef.name);
                            // var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                            // var path           = AssetDatabase.GetAssetPath(objectRef);
                            // var sprite         = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                            // spriteRenderer.sprite = sprite;
                        }
                    }
                    // if the object is the particular asset type...
                    else if (type.BaseType == typeof(MonoBehaviour))
                    {
                        var mono = objectRef as Component;
                        // we create a new GameObject using the asset's name.
                        var gameObject = new GameObject(objectRef.name);
                        // we attach component X, associated with asset X.
                        var componentX = gameObject.AddComponent(type);
                        // we place asset X within component X.
                        //componentX.assetX = objectRef as AssetX;
                        // add to the list of selected objects.
                        selectedObjects.Add(gameObject);
                    }
                }

                // we didn't drag any assets of type AssetX, so do nothing.
                if (selectedObjects.Count == 0) return;
                // emulate selection of newly created objects.
                selectedGameObjects = selectedObjects.ToArray();
                // make sure this call is the only one that processes the event.
                Event.current.Use();
            }
        }

    #endregion
    }
}