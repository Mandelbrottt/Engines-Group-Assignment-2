using Jampacked.ProjectInca.Events;

using UnityEngine;
using UnityEngine.UI;

using Debug = UnityEngine.Debug;

namespace Jampacked.ProjectInca.Sandbox
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

		private void Update()
		{
			if (Input.anyKey)
			{
				m_dispatcher.PostEvent(new OnKeyPressed());
			}
		}

		public void Foo()
		{
			var evt = new OnClickEvent
			{
				a = 2,
			};
			
			// By default, dispatches the event during the dispatcher's next available Update()
			m_dispatcher.PostEvent(evt);
		}

		public void Bar()
		{
			var evt = new OnClick2Event
			{
				position = Input.mousePosition,
			};

			// You can manually change during what update cycle the event will be dispatched
			m_dispatcher.PostEvent(evt, DispatchBehaviour.OnLateUpdate);
		}
	}
}
