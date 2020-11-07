using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class TargetDummy : MonoBehaviour
	{
		[SerializeField]
		Color onHitColor = Color.red;
		
		[SerializeField]
		float onHitColorChangeDuration = 0.2f;

		Health m_health;

		MeshRenderer m_meshRenderer;
		
		Color m_initialColor;

		List<Color> originalColors = new List<Color>();

		void Start()
		{
			m_meshRenderer = GetComponent<MeshRenderer>();
			m_health       = GetComponent<Health>();

			m_health.onDamaged += OnDamaged;
			m_health.onDie     += OnDie;

			if (m_meshRenderer)
			{
				m_initialColor = m_meshRenderer.material.color;
			}

			MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in childRenderers)
			{
				foreach (Material mat in renderer.materials)
				{
					originalColors.Add(mat.color);
				}
			}
		}

		void OnDamaged(float a_damageAmount)
		{
			if (m_meshRenderer)
			{
				m_meshRenderer.material.color = onHitColor;
			}

			MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in childRenderers)
			{
				foreach (Material mat in renderer.materials)
				{
					mat.color = onHitColor;
				}
			}

			StartCoroutine(ColorChangeRoutine());
		}

		void OnDie()
		{
			m_health.currentHealth = m_health.maxHealth;
		}

		IEnumerator ColorChangeRoutine()
		{
			yield return new WaitForSeconds(onHitColorChangeDuration);

			if (m_meshRenderer)
			{
				m_meshRenderer.material.color = m_initialColor;
			}

			MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < childRenderers.Length; i++)
			{
				for (int j = 0; j < childRenderers[i].materials.Length; j++)
				{
					childRenderers[i].materials[j].color = originalColors[i + j];
				}
			}
		}
	}
}