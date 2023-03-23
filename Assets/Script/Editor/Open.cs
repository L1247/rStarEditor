#region

using JD.AssetizerEditor;
using UnityEditor;
using UnityEngine;

#endregion

namespace DefaultNamespace
{
    public class Open
    {
    #region Private Methods

        // Add a menu item named "Do Something" to MyMenu in the menu bar.
        [MenuItem("MyMenu/Do Something")]
        private static void DoSomething()
        {
            Debug.Log("DoSomething");
            var test2 = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Data/ItemData1.asset");
            OpenPropertiesEditorWindowDoubleClickListener.OpenInPropertyEditor(test2);
        }

    #endregion
    }
}