using Jampacked.ProjectInca;

using UnityEngine;

using Jampacked.ProjectInca.Events;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class EventTest : MonoBehaviour
	{
		private void Start()
		{
			EventDispatcherSingleton.Instance.AddListener<JumpEvent>(OnPlayerJumped);
		}

		private void OnDestroy()
		{
			EventDispatcherSingleton.Instance.RemoveListener<JumpEvent>(OnPlayerJumped);
		}

		private void OnPlayerJumped(in Event a_evt)
		{
			if (a_evt is JumpEvent evt)
			{
				Debug.Log($"Player jumped! It was {evt.sender}!");
			}
		}
	}
}