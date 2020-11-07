using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class WeaponHolder : MonoBehaviour
	{
		[SerializeField]
		private List<Weapon> carriedWeapons = new List<Weapon>();

		[Header("Audio")]
		[SerializeField]
		private AudioSource weaponAudioSource = null;

		[Header("UI")]
		[SerializeField]
		private TMP_Text currentReserveAmmoDisplay = null;

		[SerializeField]
		private TMP_Text currentClipAmmoDisplay = null;

		[SerializeField]
		private TMP_Text maxClipAmmoDisplay = null;

		private PlayerController m_playerController;

		private RecoilController m_recoilController;

		private WeaponSway m_swayController;

		private BobbingWithPlayerMovement m_bobbingController;

		private Transform m_weaponParent;

		private Transform m_fppAnalog;

		private Transform m_transform;

		private Camera m_mainCamera;

		private Camera m_weaponCamera;

		private int m_activeWeaponIndex = 1;

		private Weapon m_activeWeapon;

		public Weapon ActiveWeapon
		{
			get { return m_activeWeapon; }
		}

		public PlayerController PlayerControl
		{
			get { return m_playerController; }
		}

		private void Awake()
		{
			m_transform = transform;

			var refs = GetComponentInParent<PlayerReferences>();

			m_playerController = refs.PlayerController;
			m_recoilController = refs.RecoilController;

			m_swayController    = refs.WeaponSwayController;
			m_bobbingController = refs.WeaponBobbingController;

			m_mainCamera   = refs.MainCamera;
			m_weaponCamera = refs.WeaponCamera;

			m_weaponParent = refs.WeaponParent;

			m_fppAnalog = refs.FppAnalog;
		}

		private void Start()
		{
			foreach (Weapon weapon in carriedWeapons)
			{
				SetWeaponProperties(weapon);

				weapon.gameObject.SetActive(false);
			}

			SwapToWeaponSlot(m_activeWeaponIndex);

			//m_activeWeapon = carriedWeapons[m_activeWeaponIndex];
			//m_activeWeapon.gameObject.SetActive(true);
		}

		private void Update()
		{
			currentReserveAmmoDisplay.text = m_activeWeapon.CurrentReserveAmmo.ToString();
			currentClipAmmoDisplay.text    = m_activeWeapon.CurrentClipAmmo.ToString();
			maxClipAmmoDisplay.text        = m_activeWeapon.MaxClipAmmo.ToString();
		}

		private void SetWeaponProperties(Weapon a_weapon)
		{
			// TODO: move all this into a class to minimize the assignments here?
			//a_weapon.Holder            = this;
			a_weapon.WeaponAudioSource = weaponAudioSource;
			//a_weapon.MainCamera        = m_mainCamera;
			//a_weapon.WeaponCamera      = m_weaponCamera;
			//a_weapon.RecoilControl     = m_recoilController;

			a_weapon.transform.parent = transform;
		}

		public void AddWeapon(Weapon a_weapon, bool a_setAsActive)
		{
			SetWeaponProperties(a_weapon);

			carriedWeapons.Add(a_weapon);

			if (a_setAsActive)
			{
				SwapToWeaponSlot(carriedWeapons.IndexOf(a_weapon) + 1);
			}
		}

		// Todo [Mat]: Consider changing the return type to void? The callbacks will never utilize the bools
		public bool FireActiveWeapon(Vector3 a_fireStartPosition, Vector3 a_fireDirection, bool a_isFireHeldDown)
		{
			if (!a_isFireHeldDown || (a_isFireHeldDown && m_activeWeapon.IsAutomatic))
			{
				return m_activeWeapon.FireWeapon(a_fireStartPosition, a_fireDirection);
			}

			return false;
		}

		public bool ReloadActiveWeapon()
		{
			return m_activeWeapon.Reload();
		}

		public bool ToggleAimDownSightActiveWeapon()
		{
			return m_activeWeapon.ToggleAimDownSight();
		}

		public bool AimActiveWeapon(bool a_aimIn)
		{
			return a_aimIn
				       ? m_activeWeapon.AimIn()
				       : m_activeWeapon.AimOut();
		}

		public void SwapToWeaponSlot(int a_weaponSlotNumber)
		{
			int weaponIndex = a_weaponSlotNumber - 1;

			if (weaponIndex < 0 || weaponIndex >= carriedWeapons.Count || weaponIndex == m_activeWeaponIndex)
			{
				return;
			}

			var currWeapon = carriedWeapons[m_activeWeaponIndex];

			if (currWeapon != null)
			{
				currWeapon.AimOut();
				
				currWeapon.gameObject.SetActive(false);

				currWeapon.transform.SetParent(transform, false);
			}

			m_activeWeaponIndex = weaponIndex;

			var newWeapon = carriedWeapons[weaponIndex];

			if (newWeapon != null)
			{
				newWeapon.gameObject.SetActive(true);

				m_fppAnalog.localPosition = newWeapon.fppTransform.localPosition;
				//m_fppAnalog.localRotation = newWeapon.fppTransform.localRotation;
				m_fppAnalog.localScale    = newWeapon.fppTransform.localScale;

				var newWepTransform = newWeapon.transform;

				newWepTransform.localScale    = Vector3.one;
				newWepTransform.localPosition = Vector3.zero;
				
				newWepTransform.SetParent(m_weaponParent, true);

				newWepTransform.localRotation = Quaternion.identity;
				
				m_activeWeapon = newWeapon;

				m_activeWeapon.OnWeaponSwappedTo();

				m_swayController.swayProps       = newWeapon.swayProps;
				m_bobbingController.BobbingProps = newWeapon.swayProps.Bobbing;

			}
		}
	}
}
