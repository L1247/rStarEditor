#region

using JD.AssetizerEditor;
using Script.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#endregion

namespace rStar.Editor
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
            if (InternalEditorUtility.isApplicationActive == false) return;
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

                DrawQuad(rect , Color.red);
                // const int width = 100;
                // rect.x     += rect.width - width;
                // rect.width =  width;
                // GUI.Label(rect , guid);
                // if (GUI.Button(rect , "-")) PopUpAssetInspector.Create(asset);

                if (middleMouseDown)
                        // PopUpAssetInspector.Create(asset);
                    OpenPropertiesEditorWindowDoubleClickListener.OpenInPropertyEditor(asset);
                if (ProjectSetting.instance.DisplayContentOnMouseHover && e.modifiers == EventModifiers.Alt)
                    PopUpAssetInspector.Create(asset);
            }

            EditorApplication.RepaintProjectWindow();

            // }
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

    #endregion
    }
}