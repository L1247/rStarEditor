#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kogane;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#endregion

namespace Main
{
    [InitializeOnLoad]
    internal static class AssemblyDefinitionAssetHeaderGUI
    {
    #region Private Variables

        private sealed class AssemblyDefinitionAssetInfo
        {
        #region Public Variables

            public string GUID { get; }

            public string Name { get; }

        #endregion

        #region Constructor

            public AssemblyDefinitionAssetInfo(string guid)
            {
                var guidToAssetPath         = AssetDatabase.GUIDToAssetPath(guid);
                var assemblyDefinitionAsset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(guidToAssetPath);
                var assetExist              = assemblyDefinitionAsset != null;
                var jsonAssemblyDefinition  = JsonUtility.FromJson<JsonAssemblyDefinition>(assemblyDefinitionAsset.text);
                Name = jsonAssemblyDefinition.name;
                GUID = guid;
            }

        #endregion
        }

        private sealed class Data : ICheckBoxWindowData
        {
        #region Public Variables

            public bool   IsChecked { get; set; }
            public string Name      { get; }

        #endregion

        #region Constructor

            public Data(string reference , bool isChecked)
            {
                var isGUID            = reference.Contains("GUID:");
                if (isGUID) reference = reference.Replace("GUID:" , string.Empty);
                Name      = reference;
                IsChecked = isChecked;
            }

        #endregion
        }

    #endregion

    #region Constructor

        static AssemblyDefinitionAssetHeaderGUI()
        {
            Editor.finishedDefaultHeaderGUI -= OnGUI;
            Editor.finishedDefaultHeaderGUI += OnGUI;
        }

    #endregion

    #region Private Methods

        private static void OnGUI(Editor editor)
        {
            var assemblyDefinitionImporter = editor.target as AssemblyDefinitionImporter;

            if (assemblyDefinitionImporter == null) return;

            var assetPath              = assemblyDefinitionImporter.assetPath;
            var json                   = File.ReadAllText(assetPath);
            var jsonAssemblyDefinition = JsonUtility.FromJson<JsonAssemblyDefinition>(json);
            var useGUIDs               = Array.Exists(jsonAssemblyDefinition.references , reference => reference.Contains("GUID:"));

            var referenceArray = jsonAssemblyDefinition.references.ToArray();
            var assemblyDefinitionAssets = AssetDatabase
                                          .FindAssets($"t:{nameof(AssemblyDefinitionAsset)}")
                                          .Select(guid => new AssemblyDefinitionAssetInfo(guid))
                                          .OrderBy(info => info.Name)
                                          .ToArray();

            // foreach (var s in referenceArray) Debug.Log($"{s}");
            // Debug.Log($"{referenceArray.Length} {assemblyDefinitionAssets.Length}");
            // foreach (var assemblyDefinitionAsset in assemblyDefinitionAssets)
            // {
            // Debug.Log($"{assemblyDefinitionAsset.Name} , {assemblyDefinitionAsset}");
            // }

            if (GUILayout.Button("Select References"))
            {
                var dataArray = new Data[] { };
                if (useGUIDs)
                    dataArray = assemblyDefinitionAssets
                               .Select(info =>
                                               new Data(info.Name , referenceArray.Contains($"GUID:{info.GUID}")))
                               .ToArray();
                else
                    dataArray = assemblyDefinitionAssets
                               .Select(info => info.Name)
                               .Select(assemblyName =>
                                               new Data(assemblyName , referenceArray.Contains(assemblyName)))
                               .ToArray();

                CheckBoxWindow.Open
                        (
                        "Select References" ,
                        dataArray ,
                        OnOk
                        );

                void OnOk(IReadOnlyList<ICheckBoxWindowData> _)
                {
                    var oldReferences = jsonAssemblyDefinition.references.ToArray();

                    var newReferences = dataArray
                                       .Where(x => x.IsChecked)
                                       .Select(x => $"{x.Name}")
                                       .ToArray();

                    if (oldReferences.SequenceEqual(newReferences)) return;

                    jsonAssemblyDefinition.references = newReferences;

                    var newJson = JsonUtility.ToJson(jsonAssemblyDefinition , true);

                    File.WriteAllText(assetPath , newJson , Encoding.UTF8);
                    AssetDatabase.Refresh();
                }
            }
        }

    #endregion
    }
}