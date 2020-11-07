using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class WeaponSway : MonoBehaviour
	{
		//[SerializeField]
		//private WeaponSwayProps swayProps = null;

		[HideInInspector]
		public WeaponSwayProps swayProps = null;

		private const float MAX_HORIZONTAL_SWAY_VELOCITY = 16f;
		private const float MAX_VERTICAL_SWAY_VELOCITY   = 13f;

		private PlayerMotor              m_playerMotor;
		private PlayerRotationController m_playerRotationController;

		private Transform m_swayTarget;

		private Vector3    m_initialPosition;
		private Quaternion m_initialRotation;

		private void Awake()
		{
			var refs = GetComponentInParent<PlayerReferences>();

			m_swayTarget = refs.WeaponSwayTarget;

			m_playerMotor = refs.PlayerMotor;

			m_playerRotationController = refs.RotationController;
		}

		private void Start()
		{
			m_initialPosition = m_swayTarget.localPosition;
			m_initialRotation = m_swayTarget.localRotation;
			
			// TEMPORARY: This creates a memory leak i (Myles) think
			swayProps = ScriptableObject.CreateInstance<WeaponSwayProps>();

			//swayProps.PositionalSmoothingStrength =
			//	1f / swayProps.PositionalSmoothingStrength; //since the value is actually the inverse of smoothing
			//swayProps.RotationalSmoothingStrength =
			//	1f / swayProps.RotationalSmoothingStrength; //since the value is actually the inverse of smoothing
		}

		private void Update()
		{
			Vector3 swayVelocity = CalculateRelativeSwayVelocity();

			ApplySway(swayVelocity, swayProps.LookPositional, swayProps.MovePositional, SwayType.Positional);
			ApplySway(swayVelocity, swayProps.LookRotational, swayProps.MoveRotational, SwayType.Rotational);
		}

		//calculates sway velocity relative to the maximum (so each vector component will be between 0 and 1)
		private Vector3 CalculateRelativeSwayVelocity()
		{
			// TODO: Get a reference to the one we already calculate in playerMotor
			Vector3 playerLocalVelocity =
				m_playerMotor.transform.InverseTransformDirection(
					m_playerMotor.Velocity
				); //get relative velocity from the inverse transform direction

			//since the player's grounded velocity may not be zero, we need to account for it
			if (m_playerMotor.IsGrounded)
			{
				playerLocalVelocity.y -= m_playerMotor.GetGroundedVelocityY();
			}

			Vector3 relativeSwayVelocity;
			relativeSwayVelocity.x = Mathf.Min(playerLocalVelocity.x / MAX_HORIZONTAL_SWAY_VELOCITY, 1f);
			relativeSwayVelocity.y = Mathf.Min(playerLocalVelocity.y / MAX_VERTICAL_SWAY_VELOCITY, 1f);
			relativeSwayVelocity.z = Mathf.Min(playerLocalVelocity.z / MAX_HORIZONTAL_SWAY_VELOCITY, 1f);

			if (m_playerMotor.IsWallRunning)
			{
				relativeSwayVelocity.y = 0f; //do not apply vertical sway if the player is wall running
			}

			return relativeSwayVelocity;
		}

		private void ApplySway(
			Vector3                            a_relativeSwayVelocity,
			WeaponSwayProps.LookBasedSwayProps a_lookBasedSway,
			WeaponSwayProps.MoveBasedSwayProps a_moveBasedSway,
			SwayType                           a_swayType
		)
		{
			float posNegative = a_swayType == SwayType.Positional ? -1 : 1;
			float rotNegative = a_swayType == SwayType.Rotational ? -1 : 1;

			//calculate look-based sway
			Vector2 lookSway = new Vector2(
				m_playerRotationController.CurrentFrameMouseX * a_lookBasedSway.swayStrength.x * rotNegative,
				m_playerRotationController.CurrentFrameMouseY * a_lookBasedSway.swayStrength.y * -1f
			);

			//calculate movement-based sway
			Vector3 moveSway = new Vector3(
				a_relativeSwayVelocity.x                                  * a_moveBasedSway.swayStrength.x,
				a_relativeSwayVelocity.y * a_moveBasedSway.swayStrength.y * posNegative,
				a_relativeSwayVelocity.z * a_moveBasedSway.swayStrength.z * posNegative
			);

			//clamp all sway values
			lookSway.x = Mathf.Clamp(lookSway.x, -a_lookBasedSway.maxSway.x, a_lookBasedSway.maxSway.x);
			lookSway.y = Mathf.Clamp(lookSway.y, -a_lookBasedSway.maxSway.y, a_lookBasedSway.maxSway.y);

			moveSway.x = Mathf.Clamp(moveSway.x, -a_moveBasedSway.maxSway.x, a_moveBasedSway.maxSway.x);
			moveSway.y = Mathf.Clamp(moveSway.y, -a_moveBasedSway.maxSway.y, a_moveBasedSway.maxSway.y);
			moveSway.z = Mathf.Clamp(moveSway.z, -a_moveBasedSway.maxSway.z, a_moveBasedSway.maxSway.z);

			switch (a_swayType)
			{
				case SwayType.Positional:
				{
					//calculate new position and apply positional sway with smoothing
					Vector3 newPosition = m_initialPosition
					                      + new Vector3(
						                      moveSway.z,
						                      lookSway.y + moveSway.y,
						                      lookSway.x + moveSway.x
					                      );

					m_swayTarget.localPosition = Vector3.Lerp(
						m_swayTarget.localPosition,
						newPosition,
						Time.deltaTime / swayProps.PositionalSmoothingStrength
					);
					break;
				}
				case SwayType.Rotational:
				{
					//calculate new rotation and apply rotational sway with smoothing
					Quaternion newRotation = m_initialRotation
					                         * Quaternion.Euler(
						                         moveSway.x,
						                         lookSway.x,
						                         moveSway.z - moveSway.y + lookSway.y
					                         );

					m_swayTarget.localRotation = Quaternion.Slerp(
						m_swayTarget.localRotation,
						newRotation,
						Time.deltaTime / swayProps.RotationalSmoothingStrength
					);
					break;
				}
			}
		}

		private enum SwayType
		{
			Positional,
			Rotational,
		}
	}
}
