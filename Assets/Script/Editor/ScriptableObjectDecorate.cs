#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    // [CustomEditor(typeof(ScriptableObject) , true)]
    public class ScriptableObjectDecorate : Editor
    {
    #region Public Methods

        public override void OnInspectorGUI()
        {
            // using (new GUILayout.HorizontalScope())
            // {
            //     if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.Height(50)) , "Ping")) Selection.activeObject = target;
            //     ExampleDragDropGUI(EditorGUILayout.GetControlRect(GUILayout.Height(50)) , target);
            // }

            base.OnInspectorGUI();
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

        private void OnEnable()
        {
            // Debug.Log("ScriptableObject");
        }

    #endregion
    }

    // [CustomEditor(typeof(TextureImporter) , true)]
    // public class MyTest2 : Editor
    // {
    // #region Public Methods
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         if (GUILayout.Button("Adding this button")) Debug.Log("Adding this button");
    //     }
    //
    // #endregion
    // }
}