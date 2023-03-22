#region

using JD.AssetizerEditor;
using UnityEditor;
using UnityEngine;

#endregion

namespace OpenProperties
{
    [InitializeOnLoad]
    public static class ClickProperty
    {
    #region Constructor

        static ClickProperty()
        {
            EditorApplication.projectWindowItemOnGUI += DrawAssetDetails;
        }

    #endregion

    #region Unity events

        private static void Update() { }

    #endregion

    #region Private Methods

        private static void Cleanup()
        {
            Debug.Log("hierarchyChanged");
        }

        private static void DrawAssetDetails(string guid , Rect rect)
        {
            // Debug.Log($"{guid}");
            var e = Event.current;
            // if (e.isMouse)
            // {
            var middleMouseDown = e.button == 2 && e.type == EventType.MouseDown;
            var mousePosition   = e.mousePosition;
            var contains        = rect.Contains(mousePosition);

            if (contains)
            {
                var path  = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                // Debug.Log($"{asset}");
                // Right align label:
                const int width = 100;
                rect.x     += rect.width - width;
                rect.width =  width;
                // GUI.Label(rect , guid);
                if (GUI.Button(rect , "-")) PopUpAssetInspector.Create(asset);

                if (middleMouseDown)
                        // PopUpAssetInspector.Create(asset);
                    OpenPropertiesEditorWindowDoubleClickListener.OpenInPropertyEditor(asset);
            }

            EditorApplication.RepaintProjectWindow();
            // }
        }

    #endregion
    }
}