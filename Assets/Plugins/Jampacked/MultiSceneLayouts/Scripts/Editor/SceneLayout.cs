using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEditorInternal;

using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class SceneLayout
	{
		private ReorderableList m_rlist;

		public ReorderableList RList
		{
			get { return m_rlist; }
		} 
		
		[NonSerialized]
		public string title;

		public List<SceneAsset> scenes = new List<SceneAsset>();

		private TextAsset m_fileHandle;

		private readonly MainWindow m_mainWindow;

		public bool HasFileHandle
		{
			get { return m_fileHandle != null; }
		}

		public SceneLayout()
		{
		}
		
		public SceneLayout(MainWindow a_window)
		{
			m_mainWindow = a_window;
			
			GenerateReorderableList();
		}

		public SceneLayout(MainWindow a_window, TextAsset a_file)
		{
			m_mainWindow = a_window;
			
			if (a_file != null)
			{
				m_fileHandle = a_file;
				
				Load();
			}
		}

		public void Apply()
		{
			if (scenes.Count <= 0)
			{
				return;
			}

			var loadedScenes = 0;
			for (var i = 0; i < scenes.Count; i++)
			{
				var sceneAsset = scenes[i];
				if (sceneAsset != null)
				{
					var path = AssetDatabase.GetAssetOrScenePath(sceneAsset);

					var mode = loadedScenes == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive;

					EditorSceneManager.OpenScene(path, mode);

					loadedScenes++;
				}
			}
		}

		public void Save()
		{
			var jsonString = EditorJsonUtility.ToJson(this);

			if (m_fileHandle == null)
			{
				const string dir      = "Assets/Editor/SceneLayouts/";
				var          fileName = $"{title}.asset";

				Directory.CreateDirectory(dir);

				m_fileHandle = new TextAsset(jsonString);
				AssetDatabase.CreateAsset(m_fileHandle, dir + fileName);
			} else
			{
				var path = AssetDatabase.GetAssetPath(m_fileHandle);
				m_fileHandle = new TextAsset(jsonString);

				AssetDatabase.CreateAsset(m_fileHandle, path);
			}
		}

		public void Load()
		{
			if (m_fileHandle != null)
			{
				var layout = new SceneLayout();
				EditorJsonUtility.FromJsonOverwrite(m_fileHandle.text, layout);

				scenes = layout.scenes;
				GenerateReorderableList();
			}
		}

		public void UpdateObjectHandle()
		{
			var newPath = $"{title}.asset";

			if (m_fileHandle != null)
			{
				var path = AssetDatabase.GetAssetPath(m_fileHandle);
				var e    = AssetDatabase.RenameAsset(path, newPath);
				if (e.Length > 0)
				{
					Debug.LogError(e);
				}
			}
		}

		private void GenerateReorderableList()
		{
			m_rlist = new ReorderableList(scenes, typeof(SceneAsset))
			{
				drawElementCallback = m_mainWindow.OnSubDrawElement,
				onAddCallback       = m_mainWindow.OnSubAddElement,
				drawHeaderCallback = a_rect =>
				{
					EditorGUI.BeginChangeCheck();

					title = EditorGUI.TextField(a_rect, title);

					if (EditorGUI.EndChangeCheck()
					    && title.Length > 0)
					{
						if (m_mainWindow.IsLayoutConflicting(this))
						{
							UpdateObjectHandle();
						} else
						{
							Debug.LogError($"Invalid SceneLayout title \"{title}\"!");
						}
					}
				},
			};
		}
	}
}
