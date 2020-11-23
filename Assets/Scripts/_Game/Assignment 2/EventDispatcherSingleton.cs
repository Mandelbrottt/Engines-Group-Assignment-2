using UnityEngine;

namespace Jampacked.ProjectInca.Events
{
	public class EventDispatcherSingleton : EventDispatcher
	{
		private static EventDispatcher s_instance;

		public static EventDispatcher Instance
		{
			get
			{
				if (!s_instance)
				{
					s_instance = FindObjectOfType<EventDispatcherSingleton>();
				}
				
				return s_instance;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			
			Debug.Assert(s_instance == null || s_instance == this);
			s_instance = this;
		}
	}
}
