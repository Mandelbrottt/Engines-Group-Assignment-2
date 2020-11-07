namespace Jampacked.ProjectInca.Gameplay
{
    public class EventListener
    {
		public delegate void EventHandler(Event a_args);

		public event EventHandler Handler;

		public void Invoke(Event a_args)
		{
			Handler?.Invoke(a_args);
		}
    }
}