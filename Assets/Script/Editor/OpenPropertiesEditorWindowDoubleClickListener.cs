#region

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#endregion

namespace Script.Editor
{
    /// <summary>
    ///     Listens <see cref="OnOpenAssetAttribute" /> (order 100) for everything except folders.
    ///     Opens the asset in a new window (like an unchangeable inspector).
    ///     Forum thread: https://forum.unity.com/threads/alt-p-is-a-godsent.834703/
    /// </summary>
    public static class OpenPropertiesEditorWindowDoubleClickListener
    {
    #region Public Methods

        public static bool OpenInPropertyEditor(Object asset)
        {
            if (asset is SceneAsset) return false;
            EditorUtility.OpenPropertyEditor(asset);
            return true;
        }

    #endregion

    #region Private Methods

        /// <summary>
        ///     Listens <see cref="OnOpenAssetAttribute" /> (order 100) for everything except folders.
        /// </summary>
        /// <param name="instanceID">
        ///     <see cref="OnOpenAssetAttribute" />
        /// </param>
        /// <param name="line">
        ///     <see cref="OnOpenAssetAttribute" />
        /// </param>
        /// <returns>True if opening the asset is handled</returns>
        [OnOpenAsset(100)]
        private static bool HandleOpenAsset(int instanceID , int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj == null) return false;

            if (IsFolder(obj)) return false;

            return OpenInPropertyEditor(obj);
        }

        private static bool IsFolder(Object obj)
        {
            var assetPath = AssetDatabase.GetAssetPath(obj);
            return !string.IsNullOrEmpty(assetPath) && AssetDatabase.IsValidFolder(assetPath);
        }

    #endregion
    }
}