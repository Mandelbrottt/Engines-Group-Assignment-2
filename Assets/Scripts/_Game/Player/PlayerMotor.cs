using System;

using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class PlayerMotor : MonoBehaviour
	{
		[SerializeField]
		private PlayerMovementProps playerMovementProps = null;

		private PlayerMovementProps.PlayerMotorProps m_motorProps;

		//[Header("Wall Running")]
		//[SerializeField]
		//WallRunning wallRunning = null;

		private WallRunning m_wallRunning = null;

		// TODO: Make these properties of PlayerMovementConfig
		private const float GROUNDED_VELOCITY_Y = -2f;

		private const float COYOTE_TIME           = 0.1f;
		private const float WALL_JUMP_COYOTE_TIME = 0.15f;
		private const float JUMP_BUFFER_TIME      = 0.07f;

		private const float
			SLOPE_RIDE_DISTANCE_LIMIT =
				3f; //the max distance above a slope where the player can be considered to be "on" it

		private const float
			SLOPE_RIDE_DOWNWARDS_FORCE_STRENGTH =
				35f; //the strength of the downwards force applied to pull the player onto a slope that they're going down

		private CharacterController m_characterController;
		private Transform           m_moveTarget;

		private float m_lastTimeGrounded;
		private float m_lastTimeJumpInputted = -999f;

		private float m_currentAccelerationRate;
		private float m_currentDecelerationRate;
		private float m_currentMaxMovementSpeed;

		private float m_jumpVelocityY;

		private Vector3 m_velocity;

		public Vector3 Velocity
		{
			get { return m_velocity; }
			set { m_velocity = value; }
		}

		private Vector3 m_localVelocityCache;

		private bool m_isWallRunning = false;

		public bool IsWallRunning
		{
			get { return m_isWallRunning; }
		}

		private bool m_isGrounded;

		public bool IsGrounded
		{
			get { return m_isGrounded; }
		}

		public float JumpHeight
		{
			get { return m_motorProps.VerticalForces.jumpHeight; }
			set
			{
				UpdateJumpVelocityY(value);
				m_motorProps.VerticalForces.jumpHeight = value;
			}
		}

		public float GetGroundedVelocityY()
		{
			return GROUNDED_VELOCITY_Y;
		}

		private void UpdateJumpVelocityY(float a_newJumpHeight)
		{
			m_jumpVelocityY =
				Mathf.Sqrt(
					a_newJumpHeight * 2f * m_motorProps.VerticalForces.gravityStrength
				); //calculate velocity needed in order to reach desired jump height
		}

		private void Awake()
		{
			var refs = GetComponentInParent<PlayerReferences>();

			m_characterController = refs.CharacterController;

			//m_characterController = GetComponent<CharacterController>();

			m_moveTarget = refs.MoveTarget;

			m_wallRunning = refs.WallRunning;

			m_motorProps = playerMovementProps.Motor;
		}

		private void Start()
		{
			UpdateJumpVelocityY(m_motorProps.VerticalForces.jumpHeight);
			m_wallRunning.SetWallJumpHeight(m_wallRunning.WallJumpHeight, m_motorProps.VerticalForces.gravityStrength);
		}

		public void Move(Vector2 a_inputAxes, bool a_jump)
		{
			if (a_jump)
			{
				m_lastTimeJumpInputted = Time.time;
			}

			PerformGroundCheck();

			UpdateCurrentMovementVariables();

			float verticalAxis   = a_inputAxes.y;
			float horizontalAxis = a_inputAxes.x;

			if (m_isWallRunning)
			{
				m_wallRunning.UpdateWallRunning(
					ref m_velocity,
					m_motorProps.VerticalForces.gravityStrength,
					m_isGrounded,
					horizontalAxis
				);
			} else //is NOT wall running
			{
				m_wallRunning.CheckForWallRun(ref m_velocity, m_isGrounded, verticalAxis);
			}

			m_isWallRunning = m_wallRunning.IsWallRunning;

			if (!m_isWallRunning)
			{
				m_localVelocityCache = m_moveTarget.InverseTransformDirection(m_velocity);

				ProcessBasicMovement(verticalAxis, horizontalAxis);

				ApplyDeceleration(verticalAxis, horizontalAxis);

				//clamp magnitude of velocity on the xz plane
				Vector3 newVelocityXZ = m_velocity;
				newVelocityXZ.y = 0f;
				newVelocityXZ   = Vector3.ClampMagnitude(newVelocityXZ, m_currentMaxMovementSpeed);

				m_velocity = new Vector3(newVelocityXZ.x, m_velocity.y, newVelocityXZ.z);

				m_velocity.y -= m_motorProps.VerticalForces.gravityStrength * Time.deltaTime; //apply gravity

				//if the player is grounded, downwards velocity should be reset
				if (m_isGrounded && m_velocity.y < 0f)
				{
					m_velocity.y =
						GROUNDED_VELOCITY_Y; //don't set it to 0 or else the player might float above the ground a bit
				}
			}

			if (m_lastTimeJumpInputted + JUMP_BUFFER_TIME >= Time.time)
			{
				Jump();
			}

			m_velocity.y = Mathf.Max(m_velocity.y, -m_motorProps.VerticalForces.terminalVelocity);

			m_characterController.Move(m_velocity * Time.deltaTime);

			//reset vertical velocity if the player hits a ceiling
			if ((m_characterController.collisionFlags & CollisionFlags.Above) != 0)
			{
				m_velocity.y = 0f;
			}

			//after standard movement stuff is done, check if the player should be glued to a slope
			PerformOnSlopeLogic();

			//Debug.Log("SPEED: " + new Vector3(m_velocity.x, 0f, m_velocity.z).magnitude);
		}

		private void PerformGroundCheck()
		{
			m_isGrounded = m_characterController.isGrounded;

			if (m_isGrounded)
			{
				m_lastTimeGrounded = Time.time;
				m_wallRunning.OnPlayerGrounded();
			}
		}

		private void UpdateCurrentMovementVariables()
		{
			if (m_isGrounded)
			{
				m_currentAccelerationRate = m_motorProps.Grounded.accelerationRate;
				m_currentDecelerationRate = m_motorProps.Grounded.decelerationRate;
				m_currentMaxMovementSpeed = m_motorProps.Grounded.maxMovementSpeed;
			} else
			{
				m_currentAccelerationRate = m_motorProps.Airborne.accelerationRate;
				m_currentDecelerationRate = m_motorProps.Airborne.decelerationRate;
				m_currentMaxMovementSpeed = m_motorProps.Airborne.maxMovementSpeed;
			}
		}

		private void ProcessBasicMovement(float a_verticalAxis, float a_horizontalAxis)
		{
			//calculate movement direction
			Vector3 moveDirX = m_moveTarget.right   * a_horizontalAxis;
			Vector3 moveDirY = m_moveTarget.forward * a_verticalAxis;

			Vector3 localVelocity = m_localVelocityCache;

			//apply basic movement
			if (Mathf.Abs(a_verticalAxis) > Mathf.Abs(localVelocity.z) / m_currentMaxMovementSpeed)
			{
				m_velocity += moveDirY.normalized * m_currentAccelerationRate * Time.deltaTime;
			}
			if (Mathf.Abs(a_horizontalAxis) > Mathf.Abs(localVelocity.x) / m_currentMaxMovementSpeed)
			{
				m_velocity += moveDirX.normalized * m_currentAccelerationRate * Time.deltaTime;
			}
		}

		private void ApplyDeceleration(float a_verticalAxis, float a_horizontalAxis)
		{
			float frameDecelerationAmount = m_currentDecelerationRate * Time.deltaTime;

			Vector3 localVelocity = m_localVelocityCache;

			//apply deceleration if the axis isn't being moved on
			if (Mathf.Abs(a_verticalAxis) <= Mathf.Abs(localVelocity.z) / m_currentMaxMovementSpeed)
			{
				if (Mathf.Abs(localVelocity.z) > frameDecelerationAmount)
				{
					m_velocity -= m_moveTarget.forward * Mathf.Sign(localVelocity.z) * frameDecelerationAmount;
				} else
				{
					m_velocity -= m_moveTarget.forward * localVelocity.z;
				}
			}

			if (Mathf.Abs(a_horizontalAxis) <= Mathf.Abs(localVelocity.x) / m_currentMaxMovementSpeed)
			{
				if (Mathf.Abs(localVelocity.x) > frameDecelerationAmount)
				{
					m_velocity -= m_moveTarget.right * Mathf.Sign(localVelocity.x) * frameDecelerationAmount;
				} else
				{
					m_velocity -= m_moveTarget.right * localVelocity.x;
				}
			}
		}

		private void Jump()
		{
			if (m_lastTimeGrounded + COYOTE_TIME >= Time.time)
			{
				m_velocity.y = m_jumpVelocityY;

				m_lastTimeJumpInputted = -999f; //reset last jump time
			} else if (m_wallRunning.LastTimeWallRun + WALL_JUMP_COYOTE_TIME >= Time.time)
			{
				m_wallRunning.PerformWallJump(ref m_velocity);

				m_isWallRunning        = false;
				m_lastTimeJumpInputted = -999f; //reset last jump time
			}
		}

		//this function should be called AFTER standard movement is applied for the current update
		private void PerformOnSlopeLogic()
		{
			//calculate new isGrounded since player movement was just updated
			bool wasGrounded   = m_isGrounded;
			bool newIsGrounded = m_characterController.isGrounded;

			//glue the player to the slope if they're moving down one (fixes bouncing when going down slopes)
			if (!newIsGrounded && wasGrounded && m_velocity.y < 0f)
			{
				Vector3 pointAtBottomOfPlayer =
					m_moveTarget.position - (Vector3.down * m_characterController.height / 2f);

				RaycastHit hit;
				if (Physics.Raycast(pointAtBottomOfPlayer, Vector3.down, out hit, SLOPE_RIDE_DISTANCE_LIMIT))
				{
					m_characterController.Move(Vector3.down * SLOPE_RIDE_DOWNWARDS_FORCE_STRENGTH * Time.deltaTime);
				}
			}
		}
	}
}
