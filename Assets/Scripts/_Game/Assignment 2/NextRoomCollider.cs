using UnityEngine;

using Jampacked.ProjectInca.Events;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	class RoomColliderEvent : Event<RoomColliderEvent>
	{
		public GameObject sender;
	}

	[RequireComponent(typeof(BoxCollider))]
	public class NextRoomCollider : MonoBehaviour
	{
		public int objectiveUnlock;

		private BoxCollider m_collider;

		private void Awake()
		{
			m_collider = GetComponent<BoxCollider>();
		}

		private void Start()
		{
			EventDispatcherSingleton.Instance.AddListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
		}

		private void OnDestroy()
		{
			EventDispatcherSingleton.Instance.RemoveListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
		}

		private void OnTriggerEnter(Collider a_other)
		{
			if (a_other.gameObject.layer == LayerMask.NameToLayer("Player Body"))
			{
				EventDispatcherSingleton.Instance.PostEvent(
					new RoomColliderEvent()
					{
						sender = gameObject,
					}
				);
			}
		}

		private void OnObjectiveComplete(in Event a_evt)
		{
			if (a_evt is ObjectiveCompleteEvent evt)
			{
				if (objectiveUnlock == evt.objectiveId)
				{
					m_collider.isTrigger = true;
				}
			}
		}
	}
}
