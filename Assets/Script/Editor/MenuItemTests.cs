#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    public class MenuItem : EditorWindow
    {
    #region Private Methods

        [UnityEditor.MenuItem("Tools/Test _h")]
        private static void ShowWindow()
        {
            Debug.Log($"{focusedWindow.GetType()}");
        }

    #endregion
    }
}