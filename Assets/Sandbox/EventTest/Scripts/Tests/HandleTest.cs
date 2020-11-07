using Jampacked.ProjectInca.Gameplay;

using UnityEngine;

namespace Jampacked.ProjectInca
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
		}

	    private void OnDestroy()
	    {	
			m_dispatcher.RemoveListener<OnClickEvent>(OnClick);
			m_dispatcher.RemoveListener<OnClick2Event>(OnClick2);
		}

	    private static void OnClick(Gameplay.Event a_evt)
	    {
			if (a_evt is OnClickEvent evt)
			{
				Debug.Log(evt.a);
			}
		}

	    private static void OnClick2(Gameplay.Event a_evt)
	    {
			if (a_evt is OnClick2Event evt)
			{
				Debug.Log(evt.position);
			}
		}
    }
}