using System;

using Jampacked.ProjectInca;

using UnityEngine;

using Jampacked.ProjectInca.Events;

using Event = Jampacked.ProjectInca.Events.Event;

namespace Acked
{
	public class EventTest : MonoBehaviour
	{
		private void Start()
		{
			EventDispatcherSingleton.Instance.AddListener<JumpEvent>(OnPlayerJumped);
			EventDispatcherSingleton.Instance.AddListener<WallrunEvent>(OnPlayerWallrun);
			EventDispatcherSingleton.Instance.AddListener<WeaponSwapEvent>(OnPlayerSwappedWeapon);
			EventDispatcherSingleton.Instance.AddListener<WeaponFiredEvent>(OnWeaponFired);
		}

		private void OnDestroy()
		{
			EventDispatcherSingleton.Instance.RemoveListener<JumpEvent>(OnPlayerJumped);
			EventDispatcherSingleton.Instance.RemoveListener<WallrunEvent>(OnPlayerWallrun);
			EventDispatcherSingleton.Instance.RemoveListener<WeaponSwapEvent>(OnPlayerSwappedWeapon);
			EventDispatcherSingleton.Instance.RemoveListener<WeaponFiredEvent>(OnWeaponFired);
		}

		private void OnPlayerJumped(in Event a_evt)
		{
			if (a_evt is JumpEvent evt)
			{
				Debug.Log($"Player jumped! It was {evt.sender}!");
			}
		}

		private void OnPlayerWallrun(in Event a_evt)
		{
			if (a_evt is WallrunEvent evt)
			{
				Debug.Log($"Player wallran! It was {evt.sender}!");
			}
		}

		private void OnPlayerSwappedWeapon(in Event a_evt)
		{
			if (a_evt is WeaponSwapEvent evt)
			{
				Debug.Log($"Player swapped weapons! It was to {evt.weaponType}!");
			}
		}

		private void OnWeaponFired(in Event a_evt)
		{
			if (a_evt is WeaponFiredEvent evt)
			{
				int layer = LayerMask.NameToLayer("Enemy");
				if (evt.objectHit.CompareTag("WeakSpot"))
				{
					Debug.Log("Player shot! It was a weakspot!");
				} else if (evt.objectHit.layer == layer)
				{
					Debug.Log("Player shot! It was an enemy!");
				} else
				{
					Debug.Log("Player shot! It was a miss!");
				}
			}
		}
	}
}
