using Jampacked.ProjectInca.Gameplay;

using UnityEngine;
using UnityEngine.UI;

using Debug = UnityEngine.Debug;

namespace Jampacked.ProjectInca
{
	public class DispatchTest : MonoBehaviour
	{
		public Button onClickButton;
		public Button onClick2Button;

		private EventDispatcher m_dispatcher;

		private void Awake()
		{
			onClickButton.onClick.AddListener(Foo);
			onClick2Button.onClick.AddListener(Bar);

			m_dispatcher = FindObjectOfType<EventDispatcher>();
		}

		private void OnDestroy()
		{
			onClickButton.onClick.RemoveListener(Foo);
			onClick2Button.onClick.RemoveListener(Bar);
		}

		public void Foo()
		{
			var evt = new OnClickEvent
			{
				a = 2,
			};
			
			m_dispatcher.DispatchEvent(evt);
		}

		public void Bar()
		{
			var evt = new OnClick2Event
			{
				position = Input.mousePosition,
			};

			m_dispatcher.DispatchEvent(evt);
		}
	}
}
