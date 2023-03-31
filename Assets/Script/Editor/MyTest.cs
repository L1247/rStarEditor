#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    [CustomEditor(typeof(ScriptableObject) , true)]
    public class MyTest : Editor
    {
    #region Public Methods

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Adding this button")) Debug.Log("Adding this button");
        }

    #endregion
    }
}