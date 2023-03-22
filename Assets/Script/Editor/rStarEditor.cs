#region

using UnityEditor;
using UnityEngine;

#endregion

namespace OpenProperties
{
    [InitializeOnLoad]
    public class rStarEditor : Editor
    {
    #region Public Methods

        public override void OnInspectorGUI()
        {
            Debug.Log("InitializeOnLoad");
            base.OnInspectorGUI();
        }

    #endregion

    #region Private Methods

        private void OnEnable()
        {
            Debug.Log("en");
        }

    #endregion
    }
}