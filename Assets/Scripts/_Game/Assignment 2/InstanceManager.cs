using System.Collections;

using Jampacked.ProjectInca.Events;

using UnityEngine;
using UnityEngine.SceneManagement;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class InstanceManager : MonoBehaviour
	{
		private AsyncOperation m_playerSceneUnload;
		private AsyncOperation m_levelSceneUnload;

		private void Awake()
		{
		#if !UNITY_EDITOR
			SceneManager.LoadScene("Player", LoadSceneMode.Additive);
			SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);

			SceneManager.SetActiveScene(SceneManager.GetSceneByName("Player"));
		#endif
		}

		private void Start()
		{
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
				SceneManager.LoadScene("Player", LoadSceneMode.Additive);
				SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);

				StartCoroutine(CheckPlayerIsLoaded());
					
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
	}
}
