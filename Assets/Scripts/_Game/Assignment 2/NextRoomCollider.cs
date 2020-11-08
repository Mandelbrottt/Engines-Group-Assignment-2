using UnityEngine;

using Jampacked.ProjectInca.Events;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	class RoomColliderEvent : Event<RoomColliderEvent>
	{
		public GameObject sender;
	}

	public class NextRoomCollider : MonoBehaviour
	{
		private void OnCollisionEnter(Collision a_other)
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
	}
}
