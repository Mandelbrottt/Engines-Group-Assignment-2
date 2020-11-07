using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class RecoilController : MonoBehaviour
	{
		[SerializeField]
		private float resetSpeed = 0f;

		private Transform m_recoilTarget;

		private float m_recoilSpeed;
		private float m_recoilParam = 0f;
		private float m_resetParam  = 0f;

		private Quaternion m_recoilRotation = Quaternion.identity;

		private void Start()
		{
			var refs = GetComponentInParent<PlayerReferences>();

			m_recoilTarget = refs.RecoilTarget;
		}

		private void Update()
		{
			UpdateRecoil();
		}

		public void ApplyRecoil(float a_duration, float a_recoilSpeed, Vector2 a_minRecoil, Vector2 a_maxRecoil)
		{
			//m_duration    = a_duration;
			m_recoilSpeed = a_recoilSpeed;
			m_recoilParam = 0f;
			m_resetParam  = 0f;

			Vector2 recoilAmount = new Vector2(
				Random.Range(a_minRecoil.x, a_maxRecoil.x),
				Random.Range(a_minRecoil.y, a_maxRecoil.y)
			);

			//add new recoil to the current recoil rotation
			m_recoilRotation = Quaternion.Euler(-recoilAmount.y, recoilAmount.x, 0f) * m_recoilRotation;
		}

		private void UpdateRecoil()
		{
			if (m_recoilParam < 1f)
			{
				m_recoilParam += m_recoilSpeed * Time.deltaTime;

				//m_transform.localRotation = Quaternion.Slerp(transform.localRotation, m_recoilRotation, m_recoilParam);
				m_recoilTarget.localRotation = Quaternion.Slerp(m_recoilTarget.localRotation, m_recoilRotation, m_recoilParam);
			} else if (m_resetParam < 1f)
			{
				m_resetParam += resetSpeed * Time.deltaTime;

				var localRotation = m_recoilTarget.localRotation;
				Quaternion newRotation = Quaternion.Slerp(localRotation, Quaternion.identity, m_resetParam);
				Quaternion changeInRotation =
					newRotation * Quaternion.Inverse(localRotation); //subtract old from new rotation
				//localRotation = newRotation;
				m_recoilTarget.localRotation = newRotation;

				m_recoilRotation *= changeInRotation; //reduce current recoil rotation by amount reset
			}
		}
	}
}