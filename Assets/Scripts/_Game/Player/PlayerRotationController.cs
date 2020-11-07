using UnityEngine;
using UnityEngine.InputSystem;

namespace Jampacked.ProjectInca
{
	public class PlayerRotationController : MonoBehaviour
	{
		[SerializeField]
		private Transform swivelTransform = null;

		[SerializeField]
		private Transform tiltTransform = null;

		private Transform m_activeWeaponRoot;

		private float m_yaw;

		public float Yaw
		{
			get { return m_yaw; }
			set
			{
				m_yaw = value;
			}
		}

		private float m_pitch;

		public float Pitch
		{
			get { return m_pitch; }
			set
			{
				m_pitch = Mathf.Clamp(value, -80f, 80f);
			}
		}

		private float m_roll;

		public float Roll
		{
			get { return m_roll; }
			set
			{
				m_roll = value;
			}
		}

		private float m_currentFrameMouseX;

		public float CurrentFrameMouseX
		{
			get { return m_currentFrameMouseX; }
		}

		private float m_currentFrameMouseY;

		public float CurrentFrameMouseY
		{
			get { return m_currentFrameMouseY; }
		}

		private CharacterCommandHandler m_commandHandler;

		private void Awake()
		{
			var refs = GetComponentInParent<PlayerReferences>();

			m_commandHandler = refs.CharacterCommandHandler;

			m_activeWeaponRoot = refs.ActiveWeaponRoot;
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible   = false;

			Yaw   = swivelTransform.localEulerAngles.y;
			Pitch = tiltTransform.localEulerAngles.x;
			Roll  = tiltTransform.localEulerAngles.z;
		}

		public float inputScale = 1.0f;

		private void Update()
		{
			var lookInput = m_commandHandler.LookInput;
			m_currentFrameMouseX = lookInput.x;
			m_currentFrameMouseY = lookInput.y;

			Pitch -= m_currentFrameMouseY;

			Yaw += m_currentFrameMouseX;

			swivelTransform.localRotation = Quaternion.AngleAxis(m_yaw, Vector3.up);

			tiltTransform.localRotation = 
				Quaternion.AngleAxis(m_pitch, Vector3.right) * Quaternion.AngleAxis(m_roll, Vector3.forward);

			m_activeWeaponRoot.localRotation = tiltTransform.localRotation;
		}
	}
}
