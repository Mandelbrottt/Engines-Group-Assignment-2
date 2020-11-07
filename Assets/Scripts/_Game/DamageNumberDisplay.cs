using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DamageNumberDisplay : MonoBehaviour
{
	[SerializeField]
	Color regularColor = Color.black;

	[SerializeField]
	Color weakSpotColor = Color.red;

	[SerializeField]
	float sizeMultiplier = 1f;

	[SerializeField]
	float fadeSpeed = 1f;
	
	[SerializeField]
	float positionDeltaY = 1f;
	
	const float START_ALPHA = 2f;
	const float END_ALPHA   = 0f;
	
	const float MAX_SCALE_VALUE = 0.35f;
	const float MIN_SCALE_VALUE = 0.08f;
	
	Transform m_cameraTransform;

    TMP_Text m_textDisplay;
    
    Transform m_transform;
    
    Vector3 m_initialPosition;
    Vector3 m_endPosition;
    
    float m_interpolationParam;
    
    float m_startAlpha;
    float m_endAlpha;

    public void Init(Transform a_cameraTransform, Vector3 a_hitPosition, float a_damageDealt, bool a_isWeakSpotHit)
    {
	    m_transform   = transform;
	    m_textDisplay = GetComponent<TMP_Text>();
	    
	    m_cameraTransform = a_cameraTransform;

	    m_transform.position = a_hitPosition;

	    m_initialPosition = a_hitPosition;
	    m_endPosition     = m_initialPosition + new Vector3(0f, positionDeltaY, 0f);

	    m_textDisplay.text = Mathf.RoundToInt(a_damageDealt).ToString();

	    if (a_isWeakSpotHit)
	    {
		    m_textDisplay.color = weakSpotColor;
	    } else
	    {
		    m_textDisplay.color = regularColor;
	    }
    }

    void Update()
    {
	    float distanceToViewer = Vector3.Distance(m_cameraTransform.position, m_transform.position);
	    float newScaleValue    = distanceToViewer * sizeMultiplier;

	    newScaleValue = Mathf.Clamp(newScaleValue, MIN_SCALE_VALUE, MAX_SCALE_VALUE);
	    
	    m_transform.localScale = new Vector3(newScaleValue, newScaleValue, 1f);

	    //face the camera, this looks a bit jank because just doing it normally makes it look the opposite direction
		m_transform.LookAt(2f * m_transform.position - m_cameraTransform.position);
	    
	    m_transform.position = Vector3.Lerp(m_initialPosition, m_endPosition, m_interpolationParam);

	    float newAlpha = Mathf.Lerp(START_ALPHA, END_ALPHA, m_interpolationParam);

	    Color newColor = m_textDisplay.color;
	    
	    newColor.a          = newAlpha;
	    m_textDisplay.color = newColor;

	    if (m_interpolationParam >= 1f)
	    {
		    Destroy(gameObject);
	    }
	    
		m_interpolationParam += fadeSpeed * Time.deltaTime;
    }
}
