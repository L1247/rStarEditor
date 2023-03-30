#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Script.Editor
{
    [CustomEditor(typeof(ScriptableObject) , true)]
    public class MyTest : UnityEditor.Editor
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