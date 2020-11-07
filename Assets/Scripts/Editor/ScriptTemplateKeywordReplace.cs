using System;

using UnityEditor;

using UnityEngine;

namespace Jampacked.ProjectInca.Editor
{
	public class ScriptTemplateKeywordReplace : UnityEditor.AssetModificationProcessor
	{
		// Adapted from https://stackoverflow.com/a/52395369
		public static void OnWillCreateAsset(string a_assetName)
		{
			a_assetName = a_assetName.Replace(".meta", "");

			if (!a_assetName.EndsWith(".cs"))
			{
				return;
			}

			var file = System.IO.File.ReadAllText(a_assetName);

			var nameSpace = EditorSettings.projectGenerationRootNamespace;
			file = file.Replace("#NAMESPACE#", nameSpace);

			var index    = Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal);
			var fullPath = Application.dataPath.Substring(0, index) + a_assetName;

			System.IO.File.WriteAllText(fullPath, file);
			AssetDatabase.Refresh();
		}
	}
}
