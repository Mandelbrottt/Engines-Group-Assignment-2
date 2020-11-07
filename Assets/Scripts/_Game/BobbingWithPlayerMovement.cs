using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class BobbingWithPlayerMovement : MonoBehaviour
	{
		//[SerializeField]
		//private WeaponSwayProps swayProps = null;

		private WeaponSwayProps.BobbingProps m_bobbingProps;

		public WeaponSwayProps.BobbingProps BobbingProps
		{
			get { return m_bobbingProps; }
			set
			{
				m_bobbingProps = value;

				//m_initialPositionY = m_bobbingTarget.localPosition.y;
				m_initialPositionY = 0.0f;
				m_minPositionY     = m_initialPositionY - BobbingProps.positionalAmount;
				m_maxPositionY     = m_initialPositionY + BobbingProps.positionalAmount;

				//m_initialRotationY = m_bobbingTarget.localEulerAngles.y;
				m_initialRotationY = 0.0f;
				m_minRotationY     = m_initialRotationY - BobbingProps.rotationalAmount;
				m_maxRotationY     = m_initialRotationY + BobbingProps.rotationalAmount;

				SetStartAndEndValues();
			}
		}

		private const float MAX_PLAYER_VELOCITY_MAGNITUDE = 14f;

		private PlayerMotor m_playerMotor;

		private Transform m_bobbingTarget;

		private bool m_isBobbingUp = false;

		private float m_interpolationParam = 0.5f;

		private float m_initialPositionY;
		private float m_minPositionY;
		private float m_maxPositionY;
		private float m_startPositionY;
		private float m_endPositionY;

		private float m_initialRotationY;
		private float m_minRotationY;
		private float m_maxRotationY;
		private float m_startRotationY;
		private float m_endRotationY;

		private void Awake()
		{
			//m_bobbingProps = swayProps.Bobbing;

			var refs = GetComponentInParent<PlayerReferences>();

			m_playerMotor = refs.PlayerMotor;

			m_bobbingTarget = refs.WeaponBobbingTarget;
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (m_playerMotor.IsGrounded || m_playerMotor.IsWallRunning)
			{
				ApplyBobbing();
			} else
			{
				//reset to bobbing downwards, start at the halfway point
				m_interpolationParam = 0.5f;
				m_isBobbingUp        = false;

				m_startPositionY = m_maxPositionY;
				m_endPositionY   = m_minPositionY;

				m_startRotationY = m_maxRotationY;
				m_endRotationY   = m_minRotationY;
			}
		}

		private void SetStartAndEndValues()
		{
			if (m_isBobbingUp)
			{
				m_startPositionY = m_minPositionY;
				m_endPositionY   = m_maxPositionY;

				m_startRotationY = m_minRotationY;
				m_endRotationY   = m_maxRotationY;
			} else
			{
				m_startPositionY = m_maxPositionY;
				m_endPositionY   = m_minPositionY;

				m_startRotationY = m_maxRotationY;
				m_endRotationY   = m_minRotationY;
			}
		}

		private void ApplyBobbing()
		{
			Vector3 playerVelocityXZ = m_playerMotor.Velocity;
			playerVelocityXZ.y = 0f;

			float velocityRelativeToMax = Mathf.Min(playerVelocityXZ.magnitude / MAX_PLAYER_VELOCITY_MAGNITUDE, 1f);

			m_interpolationParam += BobbingProps.speed * velocityRelativeToMax * Time.deltaTime;
			if (m_interpolationParam > 1f)
			{
				m_interpolationParam = 0f;

				m_isBobbingUp = !m_isBobbingUp;
				SetStartAndEndValues();
			}

			Vector3 newPosition = m_bobbingTarget.localPosition;
			//newPosition.x = Mathf.Lerp(m_startPositionY * 0.25f, m_endPositionY * 0.25f, m_interpolationParam);
			newPosition.y             = Mathf.Lerp(m_startPositionY, m_endPositionY, m_interpolationParam);
			m_bobbingTarget.localPosition = newPosition;

			Vector3 newRotation = m_bobbingTarget.localEulerAngles;
			//newRotation.x = Mathf.Lerp(m_startRotationY * 0.25f, m_endRotationY * 0.25f, m_interpolationParam);
			newRotation.y = Mathf.Lerp(m_startRotationY * 0.25f, m_endRotationY * 0.25f, m_interpolationParam);
			newRotation.z = Mathf.Lerp(m_startRotationY, m_endRotationY, m_interpolationParam);
			m_bobbingTarget.localRotation = Quaternion.Euler(newRotation);
		}
	}
}
