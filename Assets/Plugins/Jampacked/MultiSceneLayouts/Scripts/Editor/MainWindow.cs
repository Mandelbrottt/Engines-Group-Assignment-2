using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Jampacked.ProjectInca
{
	public class MainWindow : EditorWindow
	{
		private const string SCENCE_LAYOUTS_DIR = "Assets/Editor/SceneLayouts/";

		private ReorderableList m_masterList;

		private List<SceneLayout> m_masterIList;

		private int m_currentSceneLayoutIndex;

		private Vector2 m_mainScrollPosition;

		[MenuItem("Tools/Jampacked/Multi-Scene Layouts", priority = 1001)]
		private static void Init()
		{
			var window = GetWindow<MainWindow>();
			window.Show();
			window.titleContent = new GUIContent("Scene Layout Manager");
		}

		private void OnEnable()
		{
			m_masterIList = new List<SceneLayout>();

			m_masterList = new ReorderableList(m_masterIList, typeof(SceneLayout))
			{
				drawElementCallback = OnMasterDrawElement,
				onAddCallback       = OnMasterAddElement,
				onRemoveCallback    = OnMasterRemoveElement,
				drawHeaderCallback = a_rect =>
				{
					EditorGUI.LabelField(a_rect, "Layouts");
				},
				elementHeightCallback = OnMasterElementHeight,
			};

			TryLoadLayoutsFromFile();
		}

		private void OnGUI()
		{
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Refresh"))
				{
					TryLoadLayoutsFromFile();
				}

				if (GUILayout.Button("Save All"))
				{
					foreach (var layout in m_masterIList)
					{
						layout.Save();
					}
				}
				if (GUILayout.Button("Prune Files")
				    && EditorUtility.DisplayDialog(
					    "Prune Unreferenced Layout Files?",
					    "Are you sure you would like to prune all unreferenced Scene Layout files? This action cannot be undone.",
					    "Prune",
					    "Cancel"
				    ))
				{
					PruneUnreferencedLayoutFiles();
				}
			}
			GUILayout.EndHorizontal();

			m_mainScrollPosition = GUILayout.BeginScrollView(m_mainScrollPosition);
			{
				m_masterList.DoLayoutList();
			}
			GUILayout.EndScrollView();
		}

		private void OnMasterDrawElement(Rect a_rect, int a_index, bool a_isactive, bool a_isfocused)
		{
			m_currentSceneLayoutIndex = a_index;

			var sceneLayout = m_masterList.list[a_index] as SceneLayout;
			a_rect.y += 5;

			DrawSubList(sceneLayout, a_rect);
		}

		private void OnMasterAddElement(ReorderableList a_list)
		{
			a_list.index = a_list.count;

			var sceneLayout = new SceneLayout(this)
			{
				title = $"Scene Layout {a_list.count + 1}",
			};

			m_masterIList.Add(sceneLayout);
		}

		private void OnMasterRemoveElement(ReorderableList a_list)
		{
			var index = a_list.index;

			m_masterIList.RemoveAt(index);
		}

		private float OnMasterElementHeight(int a_index)
		{
			var list = m_masterIList[a_index].RList;

			return list.GetHeight() + 10;
		}

		public void OnSubDrawElement(Rect a_rect, int a_index, bool a_isactive, bool a_isfocused)
		{
			var index = m_currentSceneLayoutIndex;
			if (index      < 0
			    || index   >= m_masterIList.Count
			    || a_index < 0
			    || a_index >= m_masterIList[index].scenes.Count)
			{
				return;
			}

			var element = m_masterIList[index].scenes[a_index];
			a_rect.y += 2;

			var sceneAsset
				= EditorGUI.ObjectField(
					  new Rect(a_rect.x, a_rect.y, a_rect.width, EditorGUIUtility.singleLineHeight),
					  element,
					  typeof(SceneAsset),
					  true
				  ) as SceneAsset;

			bool isValid = true;
			for (int i = 0; isValid && i < m_masterIList[index].scenes.Count; i++)
			{
				if (sceneAsset == m_masterIList[index].scenes[i])
				{
					isValid = false;
				}
			}

			if (isValid)
			{
				m_masterIList[index].scenes[a_index] = sceneAsset;
			}
		}

		public void OnSubAddElement(ReorderableList a_list)
		{
			a_list.index = a_list.list.Count;

			a_list.list.Add(new Object() as SceneAsset);
		}

		public bool IsLayoutConflicting(in SceneLayout a_layout)
		{
			for (int i = 0; i < m_masterIList.Count; i++)
			{
				var element = m_masterIList[i];
				if (element          != a_layout
				    && element.title == a_layout.title)
				{
					return false;
				}
			}

			return true;
		}

		private void TryLoadLayoutsFromFile()
		{
			if (!Directory.Exists(SCENCE_LAYOUTS_DIR))
			{
				return;
			}

			var fileNames = Directory.GetFiles(SCENCE_LAYOUTS_DIR);

			foreach (var fileName in fileNames)
			{
				var asset = AssetDatabase.LoadMainAssetAtPath(fileName) as TextAsset;
				if (asset != null)
				{
					string pathWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

					int index = m_masterIList.FindIndex(a_layout => a_layout.title == pathWithoutExtension);

					var layout = new SceneLayout(this, asset)
					{
						title = pathWithoutExtension,
					};

					if (index == -1)
					{
						m_masterIList.Add(layout);
					} else
					{
						m_masterIList[index] = layout;
					}
				}
			}
		}

		private void PruneUnreferencedLayoutFiles()
		{
			if (!Directory.Exists(SCENCE_LAYOUTS_DIR))
			{
				return;
			}

			var fileNames = Directory.GetFiles(SCENCE_LAYOUTS_DIR);

			foreach (var fileName in fileNames)
			{
				var asset = AssetDatabase.LoadMainAssetAtPath(fileName) as TextAsset;
				if (asset != null)
				{
					string pathWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

					int index = m_masterIList.FindIndex(a_layout => a_layout.title == pathWithoutExtension);

					if (index == -1)
					{
						AssetDatabase.DeleteAsset(fileName);
					}
				}
			}
		}

		private void DrawSubList(SceneLayout a_sceneLayout, Rect a_rect)
		{
			Debug.Assert(a_sceneLayout != null);

			a_sceneLayout.RList.DoList(a_rect);

			a_rect.y += a_sceneLayout.RList.GetHeight() - EditorGUIUtility.singleLineHeight;

			a_rect.width  -= 70;
			a_rect.height =  EditorGUIUtility.singleLineHeight;

			var temp = a_rect;

			temp.width = a_rect.width / 3;
			if (GUI.Button(temp, "Apply"))
			{
				a_sceneLayout.Apply();
			}

			temp.x += temp.width;
			if (GUI.Button(temp, "Save"))
			{
				//SaveLayout(sceneLayout);
				a_sceneLayout.Save();
			}

			EditorGUI.BeginDisabledGroup(!a_sceneLayout.HasFileHandle);
			temp.x += temp.width;
			if (GUI.Button(temp, "Reload"))
			{
				a_sceneLayout.Load();
			}
			EditorGUI.EndDisabledGroup();
		}
	}
}
