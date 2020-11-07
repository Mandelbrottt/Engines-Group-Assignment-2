using System.Collections.Generic;

namespace Jampacked.ProjectInca.Events
{
    public class EventListener
    {
		public delegate void EventHandler(in Event a_args);

		public event EventHandler Handler;

		public void Invoke(in Event a_args)
		{
			Handler?.Invoke(in a_args);
		}
    }
}