#region

using UnityEditor;
using UnityEngine;

#endregion

namespace rStarEditor
{
    // Simple script that randomizes the rotation of the selected GameObjects.
    // It also lists which objects are currently selected.

    public class RandomizeInSelection : EditorWindow
    {
    #region Public Variables

        public float  rotationAmount = 0.33f;
        public string selected       = "";

    #endregion

    #region Private Methods

        private void OnGUI()
        {
            foreach (var t in Selection.transforms) selected += t.name + " ";

            EditorGUILayout.LabelField("Selected Object:" , selected);
            selected = "";

            if (GUILayout.Button("Randomize!")) RandomizeSelected();

            if (GUILayout.Button("Close")) Close();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void RandomizeSelected()
        {
            foreach (var transform in Selection.transforms)
            {
                var rotation = Random.rotation;
                transform.localRotation = Quaternion.Slerp(transform.localRotation , rotation , rotationAmount);
            }
        }

        [UnityEditor.MenuItem("Example/Randomize Children In Selection")]
        private static void RandomizeWindow()
        {
            var window = CreateInstance(typeof(RandomizeInSelection)) as RandomizeInSelection;
            window.ShowUtility();
        }

    #endregion
    }
}