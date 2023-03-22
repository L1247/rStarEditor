#region

using System.Collections;
using UnityEditor;
using UnityEngine;

#endregion

namespace OpenProperties
{
    /// <summary>
    ///     https://forum.unity.com/threads/custom-editor-open-new-inspector-window-and-show-asset.1167818/
    /// </summary>
    public class PopUpAssetInspector : EditorWindow
    {
    #region Private Variables

        private Object asset;
        private Editor assetEditor;
        private Color  dragColor;

    #endregion

    #region Public Methods

        public static PopUpAssetInspector Create(Object asset)
        {
            if (asset == null) return null;
            // var popUpAssetInspector = HasOpenInstances<PopUpAssetInspector>()
            // ? GetWindow<PopUpAssetInspector>()
            // /*:*/ CreateWindow<PopUpAssetInspector>($"{asset.name} | {asset.GetType().Name}");
            var popUpAssetInspector = CreateWindow<PopUpAssetInspector>($"{asset.name} | {asset.GetType().Name}");
            if (popUpAssetInspector.asset == asset) return popUpAssetInspector;

            popUpAssetInspector.asset       = asset;
            popUpAssetInspector.assetEditor = Editor.CreateEditor(asset);
            popUpAssetInspector.dragColor   = Random.ColorHSV();
            return popUpAssetInspector;
        }

    #endregion

    #region Private Methods

        private void ExampleDragDropGUI(Rect dropArea , Object obj)
        {
            // Cache References:
            var currentEvent     = Event.current;
            var currentEventType = currentEvent.type;

            // The DragExited event does not have the same mouse position data as the other events,
            // so it must be checked now:
            // if (currentEventType == EventType.DragExited)
            // DragAndDrop
            // .PrepareStartDrag(); // Clear generic data when user pressed escape. (Unfortunately, DragExited is also called when the mouse leaves the drag area)
            EditorGUI.DrawRect(dropArea , dragColor);
            if (!dropArea.Contains(currentEvent.mousePosition)) return;

            switch (currentEvent.type)
            {
                case EventType.MouseDown :
                    if (currentEvent.button == 0)
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.SetGenericData("MyData" , obj);
                        DragAndDrop.StartDrag("Dragging");
                        var objectReferences = new[] { obj };
                        DragAndDrop.objectReferences = objectReferences;
                        currentEvent.Use();
                    }

                    break;
                case EventType.DragUpdated :
                    // Determine if the mouse is over the target object
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    currentEvent.Use();

                    break;
                case EventType.DragPerform :
                    // Get the data being dropped
                    // if (DragAndDrop.objectReferences.Length > 0)
                    // {
                    //     Object droppedObject = DragAndDrop.objectReferences[0];
                    //     Debug.Log($"{droppedObject}");
                    //     DragAndDrop.AcceptDrag();
                    //     currentEvent.Use();
                    // }

                    break;
            }
        }

        private void OnGUI()
        {
            if (asset == null || assetEditor == null) return;
            GUI.enabled = false;
            asset       = EditorGUILayout.ObjectField("Asset" , asset , asset.GetType() , false);
            GUI.enabled = true;
            ExampleDragDropGUI(EditorGUILayout.GetControlRect(GUILayout.Height(100)) , asset);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            assetEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }

    #endregion

    #region Nested Types

        public class CustomDragData
        {
        #region Public Variables

            public IList originalList;
            public int   originalIndex;

        #endregion
        }

    #endregion
    }
}