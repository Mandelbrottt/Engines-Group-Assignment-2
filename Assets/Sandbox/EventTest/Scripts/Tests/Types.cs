using Jampacked.ProjectInca.Gameplay;

namespace Jampacked.ProjectInca
{
	public sealed class OnClickEvent : Event<OnClickEvent>
	{
		public int a;
	}
	
	public sealed class OnClick2Event : Event<OnClick2Event>
	{
		public UnityEngine.Vector2 position;
	}
}