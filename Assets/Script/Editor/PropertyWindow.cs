#region

using rStarEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#endregion

namespace rStar.Editor
{
    [InitializeOnLoad]
    public static class PropertyWindow
    {
    #region Constructor

        static PropertyWindow()
        {
            EditorApplication.hierarchyWindowItemOnGUI       += DrawAssetDetails;
            EditorApplication.projectWindowItemInstanceOnGUI += DrawAssetDetails;
        }

    #endregion

    #region Private Methods

        private static void DrawAssetDetails(int instanceID , Rect rect)
        {
            if (InternalEditorUtility.isApplicationActive == false) return;
            // Debug.Log($"{guid}");
            var e               = Event.current;
            var middleMouseDown = e.button == 2 && e.type == EventType.MouseDown;
            var mousePosition   = e.mousePosition;
            var contains        = rect.Contains(mousePosition);

            if (contains)
            {
                var hoveredObject = EditorUtility.InstanceIDToObject(instanceID);
                DrawQuad(rect , Color.red);
                if (middleMouseDown) PropertyEditorManager.OpenInPropertyEditor(hoveredObject);
                if (ProjectSetting.instance.DisplayContentOnMouseHover && IsAltDown())
                {
                    var single = true;
                    PropertyEditorManager.OpenInPropertyEditor(hoveredObject , single);
                    // PopUpAssetInspector.Create(hoveredObject);
                    // if (hoveredObject is GameObject gameObject)
                    // FloatingWindow.Create(gameObject);
                }
            }

            EditorApplication.RepaintProjectWindow();
        }

        private static void DrawQuad(Rect position , Color color)
        {
            var texture = new Texture2D(1 , 1);
            texture.SetPixel(0 , 0 , color);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(position , GUIContent.none);
        }

        private static void DrawQuadtexture(Rect rect)
        {
            var texture = new Texture2D(1 , 1);
            texture.SetPixel(0 , 0 , Color.red);
            texture.Apply();
            GUI.DrawTexture(rect , texture , ScaleMode.StretchToFill , false);
        }

        private static bool IsAltDown()
        {
            return Event.current.modifiers == EventModifiers.Alt;
        }

    #endregion
    }
}