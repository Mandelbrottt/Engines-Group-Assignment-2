using UnityEngine;
using UnityEngine.Events;

namespace Jampacked.ProjectInca
{
	public class Health : MonoBehaviour
	{
		public float maxHealth;

		public UnityAction<float> onDamaged;
		public UnityAction        onDie;

		[HideInInspector]
		public float currentHealth;

		void Start()
		{
			currentHealth = maxHealth;
		}

		public void TakeDamage(float a_damageAmount)
		{
			currentHealth -= a_damageAmount;
			currentHealth =  Mathf.Clamp(currentHealth, 0f, maxHealth);

			onDamaged?.Invoke(a_damageAmount);

			if (currentHealth <= 0f)
				onDie?.Invoke();
		}
	}
}