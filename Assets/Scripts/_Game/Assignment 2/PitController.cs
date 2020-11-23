using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;

using Jampacked.ProjectInca;
using Jampacked.ProjectInca.Events;

using UnityEngine;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class PitController : MonoBehaviour
	{
		private bool m_isCoroutineRunning = false;
		private bool m_tryRunCoroutine    = false;

		private int m_objectiveIndex = 0;

		private HashSet<GameObject> m_triggeredColliders = new HashSet<GameObject>();

		private CanvasManager m_canvasManager;

		private bool m_isVictoryLap = false;

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

		private float m_elapsedTime;

		private bool m_endReached;

		private int m_shots;
		private int m_hits;

		private string m_persistantPath;

		private void Awake()
		{
			EventDispatcherSingleton.Instance.AddListener<InstanceManagerInitEvent>(OnIMInit);

			m_persistantPath = Application.persistentDataPath + "/Stats.stats";
		}

		private void Start()
		{
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

			EventDispatcherSingleton.Instance.RemoveListener<InstanceManagerInitEvent>(OnIMInit);
		}

		private void Update()
		{
			if (!m_isCoroutineRunning && m_tryRunCoroutine)
			{
				StartCoroutine(CompleteCurrentObjective());
				m_tryRunCoroutine = false;
			}

			if (m_isVictoryLap)
			{
				if (m_canvasManager.secondRunUI)
				{
					m_canvasManager.secondRunUI.SetActive(true);
				}

				var go = GameObject.Find("Trigger Volumes");

				if (go != null)
				{
					for (int i = 0; i < go.transform.childCount; i++)
					{
						go.transform.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
					}
				}

				if (!m_endReached)
				{
					m_elapsedTime += Time.deltaTime;
				}

				StatsSaveLoad.Instance.LoadStats(m_persistantPath, out var bestTime, out var bestAccuracy);

				m_canvasManager.time.text = $"Time: {m_elapsedTime:F1}";

				m_canvasManager.bestTime.text
					= bestTime > 0
						  ? $"Best Time: {bestTime:F1}"
						  : "Best Time: N/A";

				m_canvasManager.accuracy.text
					= m_shots == 0
						  ? "Accuracy: N/A"
						  : $"Accuracy: {m_hits * 100 / m_shots}%";

				m_canvasManager.bestAccuracy.text
					= bestAccuracy > 0
						  ? $"Best Accuracy: {bestAccuracy}%"
						  : "Best Accuracy: N/A";
			}

			if (m_objectiveIndex >= m_objectiveStrings.Length - 1
			    && !m_isCoroutineRunning
			    && Input.GetKeyDown(KeyCode.T))
			{
				EventDispatcherSingleton.Instance.PostEvent(new SceneResetEvent());

				ResetTimer();

				m_isVictoryLap = true;
			}
		}

		private IEnumerator CompleteCurrentObjective()
		{
			m_isCoroutineRunning = true;

			m_canvasManager.currentCheckbox.sprite = m_canvasManager.completeCheckbox;

			EventDispatcherSingleton.Instance.PostEvent(
				new ObjectiveCompleteEvent()
				{
					objectiveId = m_objectiveIndex,
				}
			);

			m_objectiveIndex++;

			yield return new WaitForSecondsRealtime(1);

			Color alpha = m_canvasManager.objective1.color;

			for (int i = 9; i >= 0; i--)
			{
				alpha.a                               = i / 10f;
				m_canvasManager.objective1.color      = alpha;
				m_canvasManager.currentIcon.color     = alpha;
				m_canvasManager.currentCheckbox.color = alpha;
				yield return new WaitForSecondsRealtime(0.1f);
			}

			m_canvasManager.currentCheckbox.sprite = m_canvasManager.incompleteCheckbox;

			if (m_objectiveIndex < m_objectiveStrings.Length)
			{
				m_canvasManager.objective1.text    = m_objectiveStrings[m_objectiveIndex].ObjectiveText;
				m_canvasManager.currentIcon.sprite = m_objectiveImages[m_objectiveIndex];

				for (int i = 1; i <= 10; i++)
				{
					alpha.a                               = i / 10f;
					m_canvasManager.objective1.color      = alpha;
					m_canvasManager.currentIcon.color     = alpha;
					m_canvasManager.currentCheckbox.color = alpha;
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

				if (m_isVictoryLap && evt.sender.name == "Cube.004")
				{
					m_endReached = true;

					StatsSaveLoad.Instance.LoadStats(m_persistantPath, out var bestTime, out var bestAccuracy);

					bool doSave = false;

					if (bestTime < 0 || m_elapsedTime < bestTime)
					{
						bestTime = m_elapsedTime;
						doSave   = true;
					}

					if (m_shots != 0)
					{
						int accuracy = m_hits * 100 / m_shots;

						if (accuracy > bestAccuracy)
						{
							bestAccuracy = accuracy;
							doSave       = true;
						}
					}

					if (doSave)
					{
						StatsSaveLoad.Instance.SaveStats(m_persistantPath, bestTime, bestAccuracy);
					}
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

				if ((m_objectiveIndex    == 6 && hit.hit)
				    || (m_objectiveIndex == 7 && hit.vital))
				{
					m_tryRunCoroutine = true;
				}

				if (!m_endReached)
				{
					m_shots++;

					if (hit.hit)
					{
						m_hits++;
					}
				}
			}
		}

		private void OnIMInit(in Event a_evt)
		{
			if (a_evt is InstanceManagerInitEvent evt)
			{
				m_canvasManager = FindObjectOfType<CanvasManager>();

				m_objectiveImages = new Sprite[]
				{
					m_canvasManager.jumpIcon,
					m_canvasManager.nextRoomIcon,
					m_canvasManager.wallrunIcon,
					m_canvasManager.nextRoomIcon,
					m_canvasManager.nextRoomIcon,
					m_canvasManager.swapGunIcon,
					m_canvasManager.shootEnemyIcon,
					m_canvasManager.shootWeakspotIcon,
					m_canvasManager.nextRoomIcon,
					m_canvasManager.nextRoomIcon,
					m_canvasManager.jumpIcon,
				};

				m_canvasManager.objective1.text = m_objectiveStrings[m_objectiveIndex].ObjectiveText;
			}
		}

		private void ResetTimer()
		{
			m_shots = m_hits = 0;

			m_elapsedTime = 0;

			m_endReached = false;
		}
	}
}
