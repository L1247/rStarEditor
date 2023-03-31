#region

using UnityEditor;

#endregion

namespace rStar.Editor
{
    public class LockMenu : UnityEditor.Editor
    {
    #region Public Methods

        [MenuItem("Tools/rStarEditor/Toggle Inspector Lock %F1")] // Ctrl + L
        public static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

    #endregion
    }
}