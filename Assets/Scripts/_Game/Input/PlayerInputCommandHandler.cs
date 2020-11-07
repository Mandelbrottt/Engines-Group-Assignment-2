using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Jampacked.ProjectInca
{
	public enum InputType
	{
		Keyboard = 0,
		Gamepad  = 1,
	}

	[RequireComponent(typeof(PlayerInput))]
	public class PlayerInputCommandHandler : CharacterCommandHandler
	{
		public override event OnReloadDelegate OnReload;

		public override event OnSwapWeaponDelegate OnSwapWeapon;

		public override event OnShootDelegate OnShoot;

		public override event OnZoomDelegate OnZoom;

		InputType m_inputType = InputType.Keyboard;

		[SerializeField]
		Vector2 mouseSensitivity = Vector2.one;

		public Vector2 MouseSensitivity
		{
			get { return mouseSensitivity; }
			set
			{
				if (value.x >= 0)
				{
					mouseSensitivity.x = value.x;
				}
				if (value.y >= 0)
				{
					mouseSensitivity.y = value.y;
				}
			}
		}

		[SerializeField]
		Vector2 gamepadSensitivity = Vector2.one;

		public Vector2 GamepadSensitivity
		{
			get { return gamepadSensitivity; }
			set
			{
				if (value.x >= 0)
				{
					gamepadSensitivity.x = value.x;
				}
				if (value.y >= 0)
				{
					gamepadSensitivity.y = value.y;
				}
			}
		}

		PlayerInput m_playerInput = null;

		InputAction m_lookAction = null;
		InputAction m_moveAction = null;

		InputAction m_jumpAction = null;

		InputAction m_reloadAction      = null;
		InputAction m_shootAction       = null;
		InputAction m_weaponSwapAction1 = null;
		InputAction m_weaponSwapAction2 = null;
		InputAction m_weaponSwapAction3 = null;
		InputAction m_weaponSwapAction4 = null;
		InputAction m_zoomAction        = null;

		void Start()
		{
			m_playerInput = GetComponent<PlayerInput>();

			m_playerInput.defaultControlScheme = "Keyboard";
			m_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

			const int weaponSlot1 = 1;
			const int weaponSlot2 = 2;
			const int weaponSlot3 = 3;
			const int weaponSlot4 = 4;
			
			m_playerInput.onControlsChanged += OnControlsChanged;
			OnControlsChanged(m_playerInput);

			m_lookAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Look)];
			m_moveAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Move)];

			m_jumpAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Jump)];
			m_jumpAction.started += OnJumpAction;

			m_reloadAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Reload)];
			m_reloadAction.started += OnReloadAction;

			m_shootAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Shoot)];
			m_shootAction.performed += OnShootAction;
			
			m_weaponSwapAction1  = m_playerInput.actions[nameof(InputMaster.PlayerActions.WeaponSwap1)];
			m_weaponSwapAction1.started += a_ctx => OnSwapWeaponAction(weaponSlot1);

			m_weaponSwapAction2  = m_playerInput.actions[nameof(InputMaster.PlayerActions.WeaponSwap2)];
			m_weaponSwapAction2.started += a_ctx => OnSwapWeaponAction(weaponSlot2);

			m_weaponSwapAction3  = m_playerInput.actions[nameof(InputMaster.PlayerActions.WeaponSwap3)];
			m_weaponSwapAction3.started += a_ctx => OnSwapWeaponAction(weaponSlot3);

			m_weaponSwapAction4  = m_playerInput.actions[nameof(InputMaster.PlayerActions.WeaponSwap4)];
			m_weaponSwapAction4.started += a_ctx => OnSwapWeaponAction(weaponSlot4);

			m_zoomAction = m_playerInput.actions[nameof(InputMaster.PlayerActions.Zoom)];
			m_zoomAction.performed += OnZoomAction;
		}

		void OnControlsChanged(PlayerInput a_playerInput)
		{
			m_inputType = a_playerInput.currentControlScheme == "Gamepad" ? InputType.Gamepad : InputType.Keyboard;
		}

		void Update()
		{
			MoveInput = m_moveAction.ReadValue<Vector2>();

			if (MoveInput.sqrMagnitude > 1)
			{
				MoveInput.Normalize();
			}

			var lookInputRaw = m_lookAction.ReadValue<Vector2>();

			switch (m_inputType)
			{
				case InputType.Keyboard:
				{
					lookInputRaw *= mouseSensitivity;
					break;
				}
				case InputType.Gamepad:
				{
					lookInputRaw *= Time.deltaTime * gamepadSensitivity;
					break;
				}
			}
			
			LookInput = lookInputRaw;

			if (m_shoot)
			{
				OnShoot?.Invoke(m_shotLastFrame);
			}

			m_shotLastFrame = m_shoot;
		}

		protected override void OnJumpAction(InputAction.CallbackContext a_context)
		{
			Jump = true;
		}

		protected override void OnShootAction(InputAction.CallbackContext a_context)
		{
			m_shoot = a_context.ReadValueAsButton();
		}

		protected override void OnReloadAction(InputAction.CallbackContext a_context)
		{
			OnReload?.Invoke();
		}

		protected override void OnSwapWeaponAction(int a_input)
		{
			OnSwapWeapon?.Invoke(a_input);
		}

		protected override void OnZoomAction(InputAction.CallbackContext a_context)
		{
			OnZoom?.Invoke(a_context.ReadValueAsButton());
		}
	}
}
