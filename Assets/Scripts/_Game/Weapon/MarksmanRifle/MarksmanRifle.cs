using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class MarksmanRifle : WeaponHitscan
	{
		bool m_isBulletCycled = true;

		public override bool FireWeapon(
			Vector3 a_fireStartPosition,
			Vector3 a_fireDirection,
			bool    a_isSingleShot = true
		)
		{
			if (!m_isBulletCycled)
			{
				return false;
			}

			bool didFire = base.FireWeapon(a_fireStartPosition, a_fireDirection);

			if (!didFire)
			{
				return false;
			}

			if (m_currentClipAmmo > 0)
			{
				CycleBullet(m_firingDuration);
			}

			return true;
		}

		void CycleBullet(float a_delay = 0f)
		{
			m_isBulletCycled = false;
			
			StartCoroutine(CycleRoutine(a_delay));
		}

		IEnumerator CycleRoutine(float a_delay)
		{
			yield return new WaitForSeconds(a_delay);
			
			animatorFPP.Play("CycleBullet");
			
			yield return new WaitForSeconds(m_firingCooldown);

			m_isBulletCycled = true;
		}

		private void OnEnable()
		{
			if (!m_isBulletCycled)
			{
				CycleBullet();
			}
		}
	}
}
