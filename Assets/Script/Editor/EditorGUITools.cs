#region

using UnityEngine;

#endregion

namespace rStarEditor
{
    public static class EditorGUITools
    {
    #region Private Variables

        private static readonly Texture2D backgroundTexture = Texture2D.whiteTexture;
        private static readonly GUIStyle textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };

    #endregion

    #region Public Methods

        public static void DrawRect(Rect position , Color color , GUIContent content = null)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position , content ?? GUIContent.none , textureStyle);
            GUI.backgroundColor = backgroundColor;
        }

        public static void LayoutBox(Color color , GUIContent content = null)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Box(content ?? GUIContent.none , textureStyle);
            GUI.backgroundColor = backgroundColor;
        }

    #endregion
    }
}