using UnityEngine;
using UnityEngine.InputSystem;

namespace Jampacked.ProjectInca
{
    public abstract class CharacterCommandHandler : CommandHandler
    {
	    public delegate bool OnReloadDelegate();

	    public delegate void OnSwapWeaponDelegate(int a_index);

	    public delegate void OnShootDelegate(bool a_held);

	    public delegate bool OnZoomDelegate(bool a_zoomIn);

	    public abstract event OnReloadDelegate OnReload;

	    public abstract event OnSwapWeaponDelegate OnSwapWeapon;

	    public abstract event OnShootDelegate OnShoot;

	    public abstract event OnZoomDelegate OnZoom;

		bool m_jump = false;
		public bool Jump
		{
			get { return m_jump; }
			set { m_jump = value; }
		}

		Vector2 m_moveInput = Vector2.zero;
		public Vector2 MoveInput
		{
			get { return m_moveInput; }
			protected set { m_moveInput = value; }
		}

		Vector2 m_lookInput = Vector2.zero;
		public Vector2 LookInput
		{
			get { return m_lookInput; }
			protected set { m_lookInput = value; }
		}

		protected bool m_shoot         = false;
		protected bool m_shotLastFrame = false;

		protected abstract void OnJumpAction(InputAction.CallbackContext a_context);
	    
		protected abstract void OnShootAction(InputAction.CallbackContext a_context);
	    
	    protected abstract void OnReloadAction(InputAction.CallbackContext a_context);
	    
	    protected abstract void OnSwapWeaponAction(int a_input);

	    protected abstract void OnZoomAction(InputAction.CallbackContext a_context);
    }
}