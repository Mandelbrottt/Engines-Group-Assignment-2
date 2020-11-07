using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Jampacked.ProjectInca
{
	[RequireComponent(typeof(PlayerMotor))]
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private WeaponHolder weaponHolder = null;

		private Camera m_mainCamera = null;

		private Transform m_aimTarget = null;

		private PlayerMotor m_playerMotor = null;

		private CharacterCommandHandler m_commandHandler = null;

		private float m_initialFieldOfView = 0f;

		public float InitialFieldOfView
		{
			get { return m_initialFieldOfView; }
		}

		public float FieldOfView
		{
			get { return m_mainCamera.fieldOfView; }

			set { m_mainCamera.fieldOfView = value; }
		}

		private void Awake()
		{
			var refs = GetComponentInParent<PlayerReferences>();

			m_playerMotor = refs.PlayerMotor;

			m_commandHandler = refs.CharacterCommandHandler;

			m_mainCamera = refs.MainCamera;

			m_aimTarget = refs.AimTarget;
		}

		private void Start()
		{
			m_initialFieldOfView = m_mainCamera.fieldOfView;

			m_commandHandler.OnShoot += (a_held) =>
			{
				var transformRef = m_aimTarget;
				weaponHolder.FireActiveWeapon(transformRef.position, transformRef.forward, a_held);
			};

			m_commandHandler.OnReload += weaponHolder.ReloadActiveWeapon;

			m_commandHandler.OnSwapWeapon += weaponHolder.SwapToWeaponSlot;

			m_commandHandler.OnZoom += weaponHolder.AimActiveWeapon;
		}

		private void Update()
		{
			m_playerMotor.Move(m_commandHandler.MoveInput, m_commandHandler.Jump);
			m_commandHandler.Jump = false;
		}

		public void SetCameraZoom(float a_zoomFactor)
		{
			if (a_zoomFactor == 0f)
			{
				m_mainCamera.fieldOfView = m_initialFieldOfView;
				return;
			}

			m_mainCamera.fieldOfView = m_initialFieldOfView / a_zoomFactor;
		}
	}
}
