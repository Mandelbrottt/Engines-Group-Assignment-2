using System.Collections;
using System.Collections.Generic;

using Jampacked.ProjectInca;
using Jampacked.ProjectInca.Events;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using UnityEngine.SceneManagement;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class ObjectiveCompleteEvent : Event<ObjectiveCompleteEvent>
	{
		public int objectiveId;
	}

	public class SceneResetEvent : Event<SceneResetEvent>
	{
	}

	public class CanvasManager : MonoBehaviour
	{
		public TMP_Text objective1;
		public Sprite   jumpIcon;
		public Sprite   wallrunIcon;
		public Sprite   swapGunIcon;
		public Sprite   shootEnemyIcon;
		public Sprite   shootWeakspotIcon;
		public Sprite   incompleteCheckbox;
		public Sprite   completeCheckbox;
		public Sprite   nextRoomIcon;
		public Image    currentIcon;
		public Image    currentCheckbox;

		private bool m_isCoroutineRunning = false;
		private bool m_tryRunCoroutine    = false;

		private int m_objectiveIndex = 0;

		private HashSet<GameObject> m_triggeredColliders = new HashSet<GameObject>();

		private readonly (bool IsOnTrigger, string ObjectiveText)[] m_objectiveStrings =
		{
			(false, "Press [Space] to Jump"),
			(true, "Continue to next room"),
			(false, "Hold [W] and Jump next to a wall to Wallrun"),
			(true, "Continue to next room"),
			(true, "Wallrun the curved wall to reach the exit"),
			(false, "Press 1, 2, 3 or 4 to swap weapons"),
			(false, "Shoot the frog"),
			(false, "Shoot the frog in the head"),
			(true, "Continue to next room"),
			(true, "Play around and escape when satisfied"),
			(false, "Press T to reset the level"),
		};

		private Sprite[] m_objectiveImages;

		private void Awake()
		{
			m_objectiveImages = new Sprite[]
			{
				jumpIcon,
				nextRoomIcon,
				wallrunIcon,
				nextRoomIcon,
				nextRoomIcon,
				swapGunIcon,
				shootEnemyIcon,
				shootWeakspotIcon,
				nextRoomIcon,
				nextRoomIcon,
				jumpIcon,
			};
		}

		// Start is called before the first frame update
		private void Start()
		{
			objective1.text = m_objectiveStrings[m_objectiveIndex].ObjectiveText;

			EventDispatcherSingleton.Instance.AddListener<RoomColliderEvent>(OnRoomColliderEvent);

			EventDispatcherSingleton.Instance.AddListener<JumpEvent>(OnPlayerJumped);
			EventDispatcherSingleton.Instance.AddListener<WallrunEvent>(OnPlayerWallrun);
			EventDispatcherSingleton.Instance.AddListener<WeaponSwapEvent>(OnPlayerSwappedWeapon);
			EventDispatcherSingleton.Instance.AddListener<WeaponFiredEvent>(OnWeaponFired);
		}

		private void OnDestroy()
		{
			EventDispatcherSingleton.Instance.RemoveListener<RoomColliderEvent>(OnRoomColliderEvent);

			EventDispatcherSingleton.Instance.RemoveListener<JumpEvent>(OnPlayerJumped);
			EventDispatcherSingleton.Instance.RemoveListener<WallrunEvent>(OnPlayerWallrun);
			EventDispatcherSingleton.Instance.RemoveListener<WeaponSwapEvent>(OnPlayerSwappedWeapon);
			EventDispatcherSingleton.Instance.RemoveListener<WeaponFiredEvent>(OnWeaponFired);
		}

		// Update is called once per frame
		private void Update()
		{
			if (!m_isCoroutineRunning && m_tryRunCoroutine)
			{
				StartCoroutine(CompleteCurrentObjective());
				m_tryRunCoroutine = false;

			}
			if (m_objectiveIndex >= m_objectiveStrings.Length - 1
				&& Input.GetKeyDown(KeyCode.T))
			{
				EventDispatcherSingleton.Instance.PostEvent(new SceneResetEvent());
			}
		}

		private IEnumerator CompleteCurrentObjective()
		{
			m_isCoroutineRunning = true;

			currentCheckbox.sprite = completeCheckbox;

			EventDispatcherSingleton.Instance.PostEvent(
				new ObjectiveCompleteEvent()
				{
					objectiveId = m_objectiveIndex,
				}
			);

			m_objectiveIndex++;

			yield return new WaitForSecondsRealtime(1);

			Color alpha = objective1.color;

			for (int i = 9; i >= 0; i--)
			{
				alpha.a               = i / 10f;
				objective1.color      = alpha;
				currentIcon.color     = alpha;
				currentCheckbox.color = alpha;
				yield return new WaitForSecondsRealtime(0.1f);
			}

			currentCheckbox.sprite = incompleteCheckbox;

			if (m_objectiveIndex < m_objectiveStrings.Length)
			{
				objective1.text    = m_objectiveStrings[m_objectiveIndex].ObjectiveText;
				currentIcon.sprite = m_objectiveImages[m_objectiveIndex];

				for (int i = 1; i <= 10; i++)
				{
					alpha.a               = i / 10f;
					objective1.color      = alpha;
					currentIcon.color     = alpha;
					currentCheckbox.color = alpha;
					yield return new WaitForSecondsRealtime(0.1f);
				}
			}

			m_isCoroutineRunning = false;
		}

		private void OnRoomColliderEvent(in Event a_evt)
		{
			if (a_evt is RoomColliderEvent evt)
			{
				if (m_objectiveIndex < m_objectiveStrings.Length
				    && m_objectiveStrings[m_objectiveIndex].IsOnTrigger
				    && !m_triggeredColliders.Contains(evt.sender))
				{
					m_triggeredColliders.Add(evt.sender);
					m_tryRunCoroutine = true;
				}
			}
		}

		private void OnPlayerJumped(in Event a_evt)
		{
			if (a_evt is JumpEvent evt)
			{
				if (m_objectiveIndex == 0)
				{
					m_tryRunCoroutine = true;
				}
			}
		}

		private void OnPlayerWallrun(in Event a_evt)
		{
			if (a_evt is WallrunEvent evt)
			{
				if (m_objectiveIndex == 2)
				{
					m_tryRunCoroutine = true;
				}
			}
		}

		private void OnPlayerSwappedWeapon(in Event a_evt)
		{
			if (a_evt is WeaponSwapEvent evt)
			{
				if (m_objectiveIndex == 5)
				{
					m_tryRunCoroutine = true;
				}
			}
		}

		private void OnWeaponFired(in Event a_evt)
		{
			if (a_evt is WeaponFiredEvent evt)
			{
				(bool hit, bool vital) hit = (false, false);

				if (evt.objectHit == null)
				{
					return;
				}
				
				int layer = LayerMask.NameToLayer("Enemy");
				if (evt.objectHit.CompareTag("WeakSpot"))
				{
					hit.hit = hit.vital = true;
				} else if (evt.objectHit.layer == layer)
				{
					hit.hit = true;
				}

				if ((m_objectiveIndex == 6 && hit.hit)
				    || (m_objectiveIndex == 7 && hit.vital))
				{
					m_tryRunCoroutine = true;
				}
			}
		}
	}
}
