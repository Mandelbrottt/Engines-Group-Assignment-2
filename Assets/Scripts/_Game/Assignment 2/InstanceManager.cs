using System.Collections;

using Jampacked.ProjectInca.Events;

using UnityEngine;
using UnityEngine.SceneManagement;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class InstanceManagerInitEvent : Event<InstanceManagerInitEvent>
	{
	}

	public class InstanceManager : MonoBehaviour
	{
		private AsyncOperation m_playerSceneUnload;
		private AsyncOperation m_levelSceneUnload;

		private void Awake()
		{
		}

		private void Start()
		{
		#if !UNITY_EDITOR
			LoadScenesAdditive();
		#else
			EventDispatcherSingleton.Instance.PostEvent(new InstanceManagerInitEvent());
		#endif

			EventDispatcherSingleton.Instance.AddListener<SceneResetEvent>(OnSceneReset);
		}

		private void OnDestroy()
		{
			EventDispatcherSingleton.Instance.RemoveListener<SceneResetEvent>(OnSceneReset);
		}

		private void Update()
		{
			if (m_playerSceneUnload   != null
			    && m_levelSceneUnload != null
			    && m_playerSceneUnload.isDone
			    && m_levelSceneUnload.isDone)
			{
				LoadScenesAdditive();

				m_playerSceneUnload = m_levelSceneUnload = null;
			}
		}

		private void OnSceneReset(in Event a_event)
		{
			m_playerSceneUnload = SceneManager.UnloadSceneAsync("Player");
			m_levelSceneUnload  = SceneManager.UnloadSceneAsync("TutorialScene");
		}

		private static IEnumerator CheckPlayerIsLoaded()
		{
			var scene = SceneManager.GetSceneByName("Player");
			while (!scene.isLoaded)
			{
				yield return null;
			}

			SceneManager.SetActiveScene(scene);
		}

		private void LoadScenesAdditive()
		{
			SceneManager.LoadScene("Player", LoadSceneMode.Additive);
			SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);

			StartCoroutine(CheckPlayerIsLoaded());

			EventDispatcherSingleton.Instance.PostEvent(new InstanceManagerInitEvent());
		}
	}
}
