#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Script.Editor
{
    public class ProjectSetting : ScriptableSingleton<ProjectSetting>
    {
    #region Public Variables

        public bool DisplayContentOnMouseHover => displayContentOnMouseHover;

        [SerializeField]
        public bool displayContentOnMouseHover;

    #endregion
    }
}