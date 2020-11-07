using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Jampacked.ProjectInca.Events
{
	public enum DispatchBehaviour
	{
		OnUpdate = 0,
		OnFixedUpdate,
		OnLateUpdate,
	}

	[DisallowMultipleComponent]
	public class EventDispatcher : MonoBehaviour
	{
		private class EventQueueItem
		{
			public readonly Event   args;
			public readonly EventId id;

			public EventQueueItem(Event a_args, EventId a_id)
			{
				args     = a_args;
				id       = a_id;
			}
		}

		private readonly Dictionary<EventId, EventListener> m_listeners
			= new Dictionary<EventId, EventListener>();

		private readonly Dictionary<DispatchBehaviour, Queue<EventQueueItem>> m_eventQueue
			= new Dictionary<DispatchBehaviour, Queue<EventQueueItem>>();

		public void AddListener<TEvent>(EventListener.EventHandler a_handler)
			where TEvent : Event<TEvent>
		{
			var id = Event<TEvent>.Id;

			if (!m_listeners.TryGetValue(id, out var invoker))
			{
				invoker = new EventListener();
				m_listeners.Add(id, invoker);
			}

			invoker.Handler += a_handler;
		}

		public void RemoveListener<TEvent>(EventListener.EventHandler a_handler)
			where TEvent : Event<TEvent>
		{
			var id = Event<TEvent>.Id;

			if (m_listeners.TryGetValue(id, out var invoker) && a_handler != null)
			{
				invoker.Handler -= a_handler;
			}
		}

		public void PostEvent<TEvent>(TEvent a_args, DispatchBehaviour a_behaviour = DispatchBehaviour.OnUpdate)
			where TEvent : Event<TEvent>
		{
			if (a_args == null)
			{
				return;
			}

			var id = Event<TEvent>.Id;

		#if UNITY_EDITOR
			if (!m_eventQueue.TryGetValue(a_behaviour, out var queue))
			{
				Debug.LogError($"Invalid DispatchBehaviour!");
			} else
		#else
			var list = m_eventQueue[a_behaviour];		
		#endif
			{
				queue.Enqueue(new EventQueueItem(a_args, id));
			}
		}

		private void DispatchEvent(EventId a_id, in Event a_args)
		{
			if (m_listeners.TryGetValue(a_id, out var invoker))
			{
				invoker.Invoke(in a_args);
			}
		}

		protected virtual void Awake()
		{
			for (var i = DispatchBehaviour.OnUpdate; i <= DispatchBehaviour.OnLateUpdate; i++)
			{
				m_eventQueue[i] = new Queue<EventQueueItem>();
			}
		}

		private void Update()
		{
			DispatchCurrentQueue(DispatchBehaviour.OnUpdate);
		}

		private void FixedUpdate()
		{
			DispatchCurrentQueue(DispatchBehaviour.OnFixedUpdate);
		}

		private void LateUpdate()
		{
			DispatchCurrentQueue(DispatchBehaviour.OnLateUpdate);
		}

		private void DispatchCurrentQueue(DispatchBehaviour a_behaviour)
		{
			var eventQueue = m_eventQueue[a_behaviour];

			while (eventQueue.Count > 0)
			{
				var item = eventQueue.Dequeue();
				DispatchEvent(item.id, item.args);
			}
		}
	}
}
