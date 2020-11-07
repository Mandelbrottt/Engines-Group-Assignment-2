using Jampacked.ProjectInca.Events;

using UnityEngine;

namespace Jampacked.ProjectInca.Sandbox
{
    public class HandleTest : MonoBehaviour
    {
	    private EventDispatcher m_dispatcher;

	    private void Awake()
	    {
		    m_dispatcher = FindObjectOfType<EventDispatcher>();
	    }

	    private void Start()
	    {
		    m_dispatcher.AddListener<OnClickEvent>(OnClick);
			m_dispatcher.AddListener<OnClick2Event>(OnClick2);
			
			// No error, but this will call the OnClick2 function for the wrong event type!
			m_dispatcher.AddListener<OnKeyPressed>(OnClick2);
			m_dispatcher.AddListener<OnKeyPressed>(OnKeyPressed);
		}

	    private void OnDestroy()
	    {	
			m_dispatcher.RemoveListener<OnClickEvent>(OnClick);
			m_dispatcher.RemoveListener<OnClick2Event>(OnClick2);
			m_dispatcher.RemoveListener<OnKeyPressed>(OnClick2);
			m_dispatcher.RemoveListener<OnKeyPressed>(OnKeyPressed);
		}

	    private static void OnClick(in Events.Event a_evt)
	    {
			if (a_evt is OnClickEvent evt)
			{
				Debug.Log(evt.a);
			}
		}

	    private static void OnClick2(in Events.Event a_evt)
	    {
			if (a_evt is OnClick2Event evt)
			{
				Debug.Log(evt.position);
			}
		}
	    
	    private static void OnKeyPressed(in Events.Event a_evt)
	    {
			Debug.Log("KeyPressed");
		}
    }
}