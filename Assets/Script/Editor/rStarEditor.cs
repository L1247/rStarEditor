#region

using UnityEditor;

#endregion

namespace rStarEditor
{
    [InitializeOnLoad]
    public class rStarEditor : Editor
    {
    #region Constructor

        static rStarEditor()
        {
            ProjectSetting.instance.Load();
        }

    #endregion
    }
}