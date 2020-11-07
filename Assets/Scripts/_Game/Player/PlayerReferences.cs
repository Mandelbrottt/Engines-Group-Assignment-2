using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class PlayerReferences : MonoBehaviour
	{
		// Player's main rendering camera
		public Camera MainCamera
		{
			get { return mainCamera; }
		}

		// Camera used to render the player's current weapon
		public Camera WeaponCamera
		{
			get { return weaponCamera; }
		}

		// Transform that moves around in the world
		public Transform MoveTarget
		{
			get { return moveTarget; }
		}

		// Built-in unity character controller component attached to the MoveTarget
		public CharacterController CharacterController
		{
			get { return characterController; }
		}

		// Handles high level character movement logic
		public PlayerController PlayerController
		{
			get { return playerController; }
		}

		// Handles low level character movement logic
		public PlayerMotor PlayerMotor
		{
			get { return playerMotor; }
		}

		// Handles character wall running logic
		public WallRunning WallRunning
		{
			get { return wallRunning; }
		}
		
		// Represents player's forward axis when shooting
		public Transform AimTarget
		{
			get { return aimTarget; }
		}

		// Controls low level player rotation and looking
		public PlayerRotationController RotationController
		{
			get { return rotationController; }
		}

		// Target for recoil adjustments to the camera
		public Transform RecoilTarget
		{
			get { return recoilTarget; }
		}

		// Handles low level recoil logic
		public RecoilController RecoilController
		{
			get { return recoilController; }
		}

		// Root transform for active weapon
		public Transform ActiveWeaponRoot
		{
			get { return activeWeaponRoot; }
		}

		// Something idk
		public Transform FppAnalog
		{
			get { return fppAnalog; }
		}

		// Parent transform for active weapon
		public Transform WeaponParent
		{
			get { return weaponParent; }
		}

		// Target for sway adjustments to weapon
		public Transform WeaponSwayTarget
		{
			get { return weaponSwayTarget; }
		}

		// Handles weapon sway logic from looking around
		public WeaponSway WeaponSwayController
		{
			get { return weaponSwayController; }
		}

		// Target for bobbing adjustments to weapon
		public Transform WeaponBobbingTarget
		{
			get { return weaponBobbingTarget; }
		}

		// Handles weapon bobbing logic from moving around
		public BobbingWithPlayerMovement WeaponBobbingController
		{
			get { return weaponBobbingController; }
		}

		// Handles high level Player input
		public CharacterCommandHandler CharacterCommandHandler
		{
			get { return characterCommandHandler; }
		}

		[Header("Cameras")]
		[SerializeField]
		private Camera mainCamera = null;

		[SerializeField]
		private Camera weaponCamera = null;

		[Header("Movement")]
		[SerializeField]
		private Transform moveTarget = null;

		[SerializeField]
		private CharacterController characterController = null;

		[SerializeField]
		private PlayerController playerController = null;

		[SerializeField]
		private PlayerMotor playerMotor = null;

		[SerializeField]
		private WallRunning wallRunning = null;

		[Header("Aiming")]
		[SerializeField]
		private Transform aimTarget = null;

		[SerializeField]
		private PlayerRotationController rotationController = null;

		[Header("Recoil")]
		[SerializeField]
		private Transform recoilTarget = null;

		[SerializeField]
		private RecoilController recoilController = null;

		[Header("Weapon")]
		[SerializeField]
		private Transform activeWeaponRoot = null;

		[SerializeField]
		private Transform fppAnalog = null;

		[SerializeField]
		private Transform weaponParent = null;

		[SerializeField]
		private Transform weaponSwayTarget = null;

		[SerializeField]
		private WeaponSway weaponSwayController = null;

		[SerializeField]
		private Transform weaponBobbingTarget = null;

		[SerializeField]
		private BobbingWithPlayerMovement weaponBobbingController = null;

		[Header("Input")]
		[SerializeField]
		private CharacterCommandHandler characterCommandHandler = null;
	}
}
