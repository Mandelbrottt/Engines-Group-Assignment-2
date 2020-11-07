using System;

using UnityEngine;

namespace Jampacked.ProjectInca
{
	[CreateAssetMenu(fileName = "New Weapon Sway Properties", menuName = "Weapon Properties/Weapon Sway")]
	public class WeaponSwayProps : ScriptableObject
	{
		// WeaponSway property that governs positional smoothing
		public float PositionalSmoothingStrength
		{
			get { return positionalSmoothingStrength; }
			set { positionalSmoothingStrength = value; }
		}

		// WeaponSway property that governs rotational smoothing
		public float RotationalSmoothingStrength
		{
			get { return rotationalSmoothingStrength; }
			set { rotationalSmoothingStrength = value; }
		}

		// WeaponSway property that governs positional look sway
		public LookBasedSwayProps LookPositional
		{
			get { return lookPositional; }
			set { lookPositional = value; }
		}

		// WeaponSway property that governs positional move sway
		public MoveBasedSwayProps MovePositional
		{
			get { return movePositional; }
			set { movePositional = value; }
		}

		// WeaponSway property that governs rotational look sway
		public LookBasedSwayProps LookRotational
		{
			get { return lookRotational; }
			set { lookRotational = value; }
		}

		// WeaponSway property that governs rotational move sway
		public MoveBasedSwayProps MoveRotational
		{
			get { return moveRotational; }
			set { moveRotational = value; }
		}

		// WeaponBobbing property that governs weapon bobbing
		public BobbingProps Bobbing
		{
			get { return bobbing; }
			set { bobbing = value; }
		}

		[Serializable]
		public class LookBasedSwayProps
		{
			public Vector2 swayStrength = Vector2.zero;
			public Vector2 maxSway      = Vector2.zero;
		}

		[Serializable]
		public class MoveBasedSwayProps
		{
			public Vector3 swayStrength = Vector3.zero;
			public Vector3 maxSway      = Vector3.zero;
		}

		[Serializable]
		public class BobbingProps
		{
			public float speed            = 0.0f;
			public float positionalAmount = 0.0f;
			public float rotationalAmount = 0.0f;
		}

		[SerializeField]
		private float positionalSmoothingStrength = 0.08f;

		[SerializeField]
		private float rotationalSmoothingStrength = 0.06f;

		[SerializeField]
		private LookBasedSwayProps lookPositional = new LookBasedSwayProps()
		{
			swayStrength = new Vector2(0.04f, 0.04f),
			maxSway      = new Vector2(0.03f, 0.03f),
		};

		[SerializeField]
		private MoveBasedSwayProps movePositional = new MoveBasedSwayProps()
		{
			swayStrength = new Vector3(0.03f, 0.04f, 0.05f),
			maxSway      = new Vector3(0.05f, 0.037f, 0.08f),
		};

		[SerializeField]
		private LookBasedSwayProps lookRotational = new LookBasedSwayProps()
		{
			swayStrength = new Vector2(4.5f, 5.8f),
			maxSway      = new Vector2(2.7f, 2.7f),
		};

		[SerializeField]
		private MoveBasedSwayProps moveRotational = new MoveBasedSwayProps()
		{
			swayStrength = new Vector3(2.7f, 4.4f, 0.6f),
			maxSway      = new Vector3(4.4f, 2.8f, 1.0f),
		};

		[SerializeField]
		private BobbingProps bobbing = new BobbingProps()
		{
			speed            = 5.2f,
			positionalAmount = 0.1f,
			rotationalAmount = 0.7f,
		};
	}
}
