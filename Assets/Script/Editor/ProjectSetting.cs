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

    #endregion

    #region Private Variables

        private const string DisplayContentOnMouseHoverKey = "DisplayContentOnMouseHover";

        [SerializeField]
        private bool displayContentOnMouseHover;

    #endregion

    #region Public Methods

        public void Load()
        {
            displayContentOnMouseHover = PlayerPrefs.GetInt(DisplayContentOnMouseHoverKey) == 1;
        }

        public void SaveSettings()
        {
            var toggleDisplayContentOnMouseHover = displayContentOnMouseHover ? 0 : 1;
            PlayerPrefs.SetInt(DisplayContentOnMouseHoverKey , toggleDisplayContentOnMouseHover);
            PlayerPrefs.Save();
        }

    #endregion
    }
}