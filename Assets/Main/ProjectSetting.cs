#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    public class ProjectSetting : ScriptableSingleton<ProjectSetting>
    {
    #region Public Variables

        public bool DisplayContentOnMouseHover => displayContentOnMouseHover;
        public bool EscToCloseWindow           => escToCloseWindow;

    #endregion

    #region Private Variables

        private const string DisplayContentOnMouseHoverKey = "DisplayContentOnMouseHover";
        private const string EscToCloseWindowKey           = "EscToCloseWindow";

        [SerializeField]
        private bool displayContentOnMouseHover;

        [SerializeField]
        private bool escToCloseWindow;

    #endregion

    #region Public Methods

        public void Load()
        {
            displayContentOnMouseHover = PlayerPrefs.GetInt(DisplayContentOnMouseHoverKey , 1) == 1;
            escToCloseWindow           = PlayerPrefs.GetInt(EscToCloseWindowKey , 1) == 1;
        }

        public void SaveSettings()
        {
            var toggleDisplayContentOnMouseHover = displayContentOnMouseHover ? 1 : 0;
            var toggleEscToCloseWindow           = escToCloseWindow ? 1 : 0;
            PlayerPrefs.SetInt(DisplayContentOnMouseHoverKey , toggleDisplayContentOnMouseHover);
            PlayerPrefs.SetInt(EscToCloseWindowKey , toggleEscToCloseWindow);
            PlayerPrefs.Save();
        }

    #endregion
    }
}