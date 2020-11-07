using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEditorInternal;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Jampacked.ProjectInca
{
	public class MultiSceneLayoutMainWindow : EditorWindow
	{
		public class SceneLayout
			//: Object
		{
			[NonSerialized]
			public ReorderableList rlist;

			[NonSerialized]
			public string title;

			public List<SceneAsset> scenes = new List<SceneAsset>();

			TextAsset m_fileHandle;
			
			public bool HasFileHandle
			{
				get { return m_fileHandle != null; }
			}

			public SceneLayout()
			{
				GenerateReorderableList();
			}

			public SceneLayout(TextAsset a_file)
			{
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
					const string dir = "Assets/Editor/SceneLayouts/";
					var fileName     = $"{ title }.asset";

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
					var e = AssetDatabase.RenameAsset(path, newPath);
					if (e.Length > 0)
					{
						Debug.LogError(e);
					}
					//AssetDatabase.Refresh();
				}
			}

			void GenerateReorderableList()
			{
				rlist = new ReorderableList(scenes, typeof(SceneAsset))
				{
					drawElementCallback = OnSubDrawElement,
					onAddCallback       = OnSubAddElement,
					drawHeaderCallback = a_rect =>
					{
						EditorGUI.BeginChangeCheck();

						title = EditorGUI.TextField(a_rect, title);

						if (EditorGUI.EndChangeCheck() && title.Length > 0)
						{
							UpdateObjectHandle();
						}
					},
				};
			}
		}

		static ReorderableList s_masterList;

		static List<SceneLayout> s_masterIList;

		static int s_currentSceneLayoutIndex;

		static Vector2 s_mainScrollPosition;

		[MenuItem("Tools/Jampacked/Multi-Scene Layouts", priority = 1001)]
		static void Init()
		{
			var window = GetWindow<MultiSceneLayoutMainWindow>();
			window.Show();
			window.titleContent = new GUIContent("Scene Layout Manager");
		}

		void OnEnable()
		{
			s_masterIList = new List<SceneLayout>();

			s_masterList = new ReorderableList(s_masterIList, typeof(SceneLayout))
			{
				drawElementCallback = OnMasterDrawElement,
				onAddCallback = OnMasterAddElement,
				onRemoveCallback = OnMasterRemoveElement,
				//onReorderCallbackWithDetails = OnMasterReorderElement,
				drawHeaderCallback = a_rect =>
				{
					EditorGUI.LabelField(a_rect, "Master");
				},
				elementHeightCallback = OnMasterElementHeight,
			};

			TryLoadLayoutsFromFile();
		}

		void OnGUI()
		{
			s_mainScrollPosition = GUILayout.BeginScrollView(s_mainScrollPosition);
			{
				s_masterList.DoLayoutList();
			}
			GUILayout.EndScrollView();
		}

		void OnMasterDrawElement(Rect a_rect, int a_index, bool a_isactive, bool a_isfocused)
		{
			s_currentSceneLayoutIndex = a_index;

			var sceneLayout = s_masterList.list[a_index] as SceneLayout;
			a_rect.y += 5;

			Debug.Assert(sceneLayout != null);
			sceneLayout.rlist.DoList(a_rect);

			a_rect.y += sceneLayout.rlist.GetHeight() - EditorGUIUtility.singleLineHeight;

			a_rect.width -= 70;
			a_rect.height = EditorGUIUtility.singleLineHeight;

			var temp = a_rect;

			temp.width = a_rect.width / 3;
			if (GUI.Button(temp, "Apply"))
			{
				sceneLayout.Apply();
			}

			temp.x += temp.width;
			if (GUI.Button(temp, "Save"))
			{
				//SaveLayout(sceneLayout);
				sceneLayout.Save();
			}

			EditorGUI.BeginDisabledGroup(!sceneLayout.HasFileHandle);
			temp.x += temp.width;
			if (GUI.Button(temp, "Reload"))
			{
				sceneLayout.Load();
			}
			EditorGUI.EndDisabledGroup();
		}

		void OnMasterAddElement(ReorderableList a_list)
		{
			a_list.index = a_list.count;

			var sceneLayout = new SceneLayout
			{
				title = $"Scene Layout {a_list.count + 1}",
			};

			s_masterIList.Add(sceneLayout);

		}

		void OnMasterRemoveElement(ReorderableList a_list)
		{
			var index = a_list.index;

			s_masterIList.RemoveAt(index);
		}

		float OnMasterElementHeight(int a_index)
		{
			var list = s_masterIList[a_index].rlist;

			return list.GetHeight() + 10;
		}

		static void OnSubDrawElement(Rect a_rect, int a_index, bool a_isactive, bool a_isfocused)
		{
			var index = s_currentSceneLayoutIndex;
			if (index < 0
				|| index >= s_masterIList.Count
				|| a_index < 0
				|| a_index >= s_masterIList[index].scenes.Count)
			{
				return;
			}

			var element = s_masterIList[index].scenes[a_index];
			a_rect.y += 2;

			var sceneAsset
				= EditorGUI.ObjectField(
					  new Rect(a_rect.x, a_rect.y, a_rect.width, EditorGUIUtility.singleLineHeight),
					  element,
					  typeof(SceneAsset),
					  true
				  ) as SceneAsset;

			bool isValid = true;
			for (int i = 0; isValid && i < s_masterIList[index].scenes.Count; i++)
			{
				if (sceneAsset == s_masterIList[index].scenes[i])
				{
					isValid = false;
				}
			}

			if (isValid)
			{
				s_masterIList[index].scenes[a_index] = sceneAsset;
			}
		}

		static void OnSubAddElement(ReorderableList a_list)
		{
			a_list.index = a_list.list.Count;

			a_list.list.Add(new Object() as SceneAsset);
		}

		static void TryLoadLayoutsFromFile()
		{
			const string dir = "Assets/Editor/SceneLayouts/";

			if (!Directory.Exists(dir))
			{
				return;
			}

			var fileNames = Directory.GetFiles(dir);

			foreach (var fileName in fileNames)
			{
				var asset = AssetDatabase.LoadMainAssetAtPath(fileName) as TextAsset;
				if (asset != null)
				{
					var layout = new SceneLayout(asset)
					{
						title = Path.GetFileNameWithoutExtension(fileName)
					};

					s_masterIList.Add(layout);
				}
			}
		}
	}
}
