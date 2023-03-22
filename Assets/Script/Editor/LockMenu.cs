#region

using UnityEditor;

#endregion

namespace OpenProperties
{
    public class LockMenu : Editor
    {
    #region Public Methods

        [MenuItem("Tools/Toggle Inspector Lock %l")] // Ctrl + L
        public static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

    #endregion
    }
}