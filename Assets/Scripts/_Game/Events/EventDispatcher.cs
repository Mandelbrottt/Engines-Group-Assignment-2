using System.Collections.Generic;

using UnityEngine;

namespace Jampacked.ProjectInca.Gameplay
{
	[DisallowMultipleComponent]
	public sealed class EventDispatcher : MonoBehaviour
	{
		private readonly Dictionary<EventId, EventListener> m_listeners
			= new Dictionary<EventId, EventListener>();

		public void AddListener<T>(EventListener.EventHandler a_handler)
			where T : Event<T>
		{
			var id = Event<T>.Id;

			if (!m_listeners.TryGetValue(id, out var invoker))
			{
				invoker = new EventListener();
				m_listeners.Add(id, invoker);
			}

			invoker.Handler += a_handler;
		}

		public void RemoveListener<T>(EventListener.EventHandler a_handler)
			where T : Event<T>
		{
			var id = Event<T>.Id;

			if (m_listeners.TryGetValue(id, out var invoker))
			{
				if (a_handler != null)
				{
					invoker.Handler -= a_handler;
				}
			}
		}

		public void DispatchEvent<T>(T a_args)
			where T : Event<T>
		{
			var id = Event<T>.Id;

			if (m_listeners.TryGetValue(id, out var invoker))
			{
				invoker.Invoke(a_args);
			}
		}
	}
}
