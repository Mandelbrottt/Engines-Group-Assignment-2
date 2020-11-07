using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jampacked.ProjectInca
{
	public class LineFade : MonoBehaviour
	{
		[SerializeField]
		private float fadeSpeed = 10f;

		[SerializeField]
		private Color color = new Color();

		LineRenderer m_lineRenderer;

		float m_interpolationParam = 0f;

		// Start is called before the first frame update
		void Start()
		{
			m_lineRenderer = GetComponent<LineRenderer>();

			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[]
				{
					new GradientColorKey(color, 0f), new GradientColorKey(color, 0.5f), new GradientColorKey(color, 1f)
				},
				new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(1f, 1f)
				}
			);
			m_lineRenderer.colorGradient = gradient;
		}

		// Update is called once per frame
		void Update()
		{
			m_interpolationParam += fadeSpeed * Time.deltaTime;

			float lineStartAlpha = Mathf.Lerp(1f, 0f, m_interpolationParam * 4f);
			float lineMidAlpha   = Mathf.Lerp(1f, 0f, m_interpolationParam * 2f);
			float lineEndAlpha   = Mathf.Lerp(1f, 0f, m_interpolationParam);

			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[]
				{
					new GradientColorKey(color, 0f), new GradientColorKey(color, 0.5f), new GradientColorKey(color, 1f)
				},
				new GradientAlphaKey[]
				{
					new GradientAlphaKey(lineStartAlpha, 0f), new GradientAlphaKey(lineMidAlpha, 0.5f),
					new GradientAlphaKey(lineEndAlpha, 1f)
				}
			);
			m_lineRenderer.colorGradient = gradient;
		}
	}
}