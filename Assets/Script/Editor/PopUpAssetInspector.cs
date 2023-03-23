#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStar.Editor
{
    /// <summary>
    ///     https://forum.unity.com/threads/custom-editor-open-new-inspector-window-and-show-asset.1167818/
    /// </summary>
    public class PopUpAssetInspector : EditorWindow
    {
    #region Private Variables

        private Object             asset;
        private UnityEditor.Editor assetEditor;

    #endregion

    #region Public Methods

        public static void Create(Object asset)
        {
            if (asset == null) return;
            var titleText = $"{asset.name} | {asset.GetType().Name}";
            var popUpAssetInspector = HasOpenInstances<PopUpAssetInspector>()
                    ? GetWindow<PopUpAssetInspector>()
                    : CreateWindow<PopUpAssetInspector>(titleText);
            popUpAssetInspector.titleContent = new GUIContent(titleText);
            if (popUpAssetInspector.asset == asset) return;

            popUpAssetInspector.asset       = asset;
            popUpAssetInspector.assetEditor = UnityEditor.Editor.CreateEditor(asset);
        }

    #endregion

    #region Private Methods

        private static void CloseThisWindow()
        {
            GetWindow<PopUpAssetInspector>().Close();
        }

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
            var textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 30 , alignment = TextAnchor.MiddleCenter };
            EditorGUI.LabelField(dropArea , "Drag Zone" , textFieldStyle);
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

        private bool IsSomethingWrong()
        {
            return asset == null || assetEditor == null;
        }

        private bool IsTargetNoExist()
        {
            return asset == null && assetEditor == null;
        }

        private void OnGUI()
        {
            if (IsTargetNoExist())
            {
                CloseThisWindow();
                return;
            }

            if (IsSomethingWrong()) return;
            GUI.enabled = false;
            asset       = EditorGUILayout.ObjectField("Asset" , asset , asset.GetType() , false);
            GUI.enabled = true;
            ExampleDragDropGUI(EditorGUILayout.GetControlRect(GUILayout.Height(100)) , asset);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            assetEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }

    #endregion
    }
}