namespace Jampacked.ProjectInca.Events
{
	public abstract class Event : System.EventArgs
	{
	}
	
    public abstract class Event<T> : Event where T : class
    {
		private static readonly EventId s_id = new EventId();
	    
	    public static EventId Id
	    {
		    get { return s_id; }
	    }
    }
}