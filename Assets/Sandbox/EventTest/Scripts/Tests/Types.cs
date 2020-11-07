using Jampacked.ProjectInca.Events;

namespace Jampacked.ProjectInca.Sandbox
{
	public sealed class OnClickEvent : Event<OnClickEvent>
	{
		public int a;
	}
	
	public sealed class OnClick2Event : Event<OnClick2Event>
	{
		public UnityEngine.Vector2 position;
	}

	public sealed class OnKeyPressed : Event<OnKeyPressed>
	{
	}
}