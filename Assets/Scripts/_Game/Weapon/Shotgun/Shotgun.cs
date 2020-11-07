using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class Shotgun : WeaponHitscan
	{
		[Header("Shotgun")]
		[SerializeField]
		int numPellets = 0;

		[SerializeField]
		float maxSpread = 0f;

		void Start()
		{
			Random.InitState(System.DateTime.Now.Millisecond); //seed random
		}

		public override bool FireWeapon(
			Vector3 a_fireStartPosition,
			Vector3 a_fireDirection,
			bool    a_isSingleShot = true
		)
		{
			if (!IsAbleToFire())
			{
				return false;
			}

			bool isLastPellet = false;

			for (int i = 0; i < numPellets; i++)
			{
				if (i == numPellets - 1)
				{
					isLastPellet = true;
				}

				float spreadX = Random.Range(-1f, 1f);
				float spreadY = Random.Range(-1f, 1f);
				float spreadZ = Random.Range(-1f, 1f);
				//normalize the spread vector to keep it conical
				Vector3 spread          = Vector3.Normalize(new Vector3(spreadX, spreadY, spreadZ)) * maxSpread;
				Vector3 pelletDirection = spread + a_fireDirection;

				base.FireWeapon(a_fireStartPosition, pelletDirection, isLastPellet);
			}

			return true;
		}
	}
}