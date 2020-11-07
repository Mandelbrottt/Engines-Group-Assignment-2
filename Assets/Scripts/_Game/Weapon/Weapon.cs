using System;
using System.Linq;
using System.Runtime.InteropServices;

using TMPro;
using UnityEngine;

namespace Jampacked.ProjectInca
{
	public abstract class Weapon : MonoBehaviour
	{
		// TODO: Make a get only property
		public WeaponSwayProps swayProps = null;

		public Transform fppTransform = null;
		
		[Header("Weapon")]
		[SerializeField]
		protected float roundsPerMinute;

		[SerializeField]
		protected bool isAutomatic;

		[SerializeField]
		protected Transform muzzleFPP;

		[SerializeField]
		protected Transform muzzleTPP;

		[Header("Bullet")]
		[SerializeField]
		protected float damagePerHit;

		[SerializeField]
		protected float weakSpotMultiplier;

		[SerializeField]
		protected float range;

		[SerializeField]
		protected LayerMask layersToIgnore;

		[Header("Ammo")]
		[SerializeField]
		protected int currentReserveAmmo;

		[SerializeField]
		protected int maxReserveAmmo;

		[SerializeField]
		protected int maxClipAmmo;

		[Header("Recoil")]
		[SerializeField]
		protected float recoilDuration;

		[SerializeField]
		protected float recoilSpeed;

		[SerializeField]
		protected Vector2 minRecoilAmount;

		[SerializeField]
		protected Vector2 maxRecoilAmount;

		[Header("Reload")]
		[SerializeField]
		protected float reloadDuration;

		[Header("Aiming")]
		[SerializeField]
		protected bool canAimDownSight;
		
		[SerializeField]
		protected float aimDownSightZoomFactor;

		[Header("Audio")]
		[SerializeField]
		protected AudioClip fireAudioClip = null;

		[Header("Animation")]
		[SerializeField]
		protected Animator animatorFPP;

		[SerializeField]
		protected Animator animatorTPP;

		[Header("Damage Number Display")]
		[SerializeField]
		protected DamageNumberDisplay damageNumberDisplayPrefab;

		protected float m_lastTimeFired = -999f;
		protected float m_firingCooldown;
		protected float m_firingDuration;

		protected int m_currentClipAmmo;

		protected bool m_isReloading       = false;
		protected bool m_isAimingDownSight = false;

		//protected WeaponHolder m_holder;
		//public WeaponHolder Holder
		//{
		//	set { m_holder = value; }
		//}

		protected AudioSource m_audioSource;
		public AudioSource WeaponAudioSource
		{
			set { m_audioSource = value; }
		}

		protected Camera m_mainCamera;
		//public Camera MainCamera
		//{
		//	set { m_mainCamera = value; }
		//}

		protected Camera m_weaponCamera;
		//public Camera WeaponCamera
		//{
		//	set { m_weaponCamera = value; }
		//}

		protected PlayerController m_playerController;

		protected RecoilController m_recoilController;
		//public RecoilController RecoilControl
		//{
		//	set { m_recoilControl = value; }
		//}

		public bool IsAutomatic
		{
			get { return isAutomatic; }
		}

		public int CurrentReserveAmmo
		{
			get { return currentReserveAmmo; }
			set { currentReserveAmmo = value; }
		}

		public int CurrentClipAmmo
		{
			get { return m_currentClipAmmo; }
			set { m_currentClipAmmo = value; }
		}

		public int MaxClipAmmo
		{
			get { return maxClipAmmo; }
			set { maxClipAmmo = value; }
		}

		private void Awake()
		{
			var refs = GetComponentInParent<PlayerReferences>();
			m_mainCamera = refs.MainCamera;
			m_weaponCamera = refs.WeaponCamera;

			m_playerController = refs.PlayerController;
			m_recoilController = refs.RecoilController;

			m_firingCooldown = 60f / roundsPerMinute;

			m_currentClipAmmo = maxClipAmmo;

			AnimationClip[] animClips = animatorFPP.runtimeAnimatorController.animationClips;
			for (int i = 0; i < animClips.Length; i++)
			{
				if (animClips[i].name == "Fire")
				{
					m_firingDuration = animClips[i].length;
					break;
				}
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			//HACK: GET RID OF THIS IT'S JUST FOR TESTING
			if (Input.GetKeyDown(KeyCode.H))
			{
				currentReserveAmmo = maxReserveAmmo;
			}
			
			// Update this value when changed in the inspector
			m_firingCooldown = 60f / roundsPerMinute;
		}

		public void OnWeaponSwappedTo()
		{
			m_isReloading = false;

			if (m_currentClipAmmo <= 0)
			{
				Reload();
			}
		}

		protected bool IsAbleToFire()
		{
			if (m_lastTimeFired + m_firingCooldown > Time.time)
			{
				return false;
			}
			if (m_currentClipAmmo <= 0 || m_isReloading)
			{
				return false;
			}

			return true;
		}

		protected bool IsAbleToReload()
		{
			if (m_currentClipAmmo == maxClipAmmo || m_isReloading || currentReserveAmmo <= 0)
			{
				return false;
			}

			return true;
		}

		protected void CreateDamageNumberPopup(Vector3 a_hitPosition, float a_damageDealt, bool a_isWeakSpotHit)
		{
			DamageNumberDisplay newDamageNumberDisplay = Instantiate(damageNumberDisplayPrefab);

			newDamageNumberDisplay.Init(m_mainCamera.transform, a_hitPosition, a_damageDealt, a_isWeakSpotHit);
		}

		public abstract bool FireWeapon(
			Vector3 a_fireStartPosition,
			Vector3 a_fireDirection,
			bool    a_isSingleShot = true
		);

		public abstract bool Reload(float a_delay = 0f);

		public bool ToggleAimDownSight()
		{
			return m_isAimingDownSight 
				       ? AimOut() 
				       : AimIn();
		}

		public bool AimIn()
		{
			if (!canAimDownSight || m_isAimingDownSight)
			{
				return false;
			}

			string currentAnimationName = animatorFPP.GetCurrentAnimatorClipInfo(0)[0].clip.name;
			if (currentAnimationName != "Idle" && currentAnimationName != "CycleBullet")
			{
				return false;
			}

			m_isAimingDownSight = true;

			animatorFPP.SetBool("IsAimingDownSight", true);
			
			// TODO: Convert to event
			m_playerController.SetCameraZoom(aimDownSightZoomFactor);

			return true;
		}

		public bool AimOut()
		{
			if (!canAimDownSight || !m_isAimingDownSight)
			{
				return false;
			}

			m_isAimingDownSight = false;

			animatorFPP.SetBool("IsAimingDownSight", false);

			// TODO: Convert to event
			m_playerController.SetCameraZoom(0f);

			return true;
		}
	}
}