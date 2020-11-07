using System;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Jampacked.ProjectInca
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class CrossRadialDeadzoneProcessor : InputProcessor<Vector2>
    {
	    [Tooltip("")]
	    public float radialMin = InputSystem.settings.defaultDeadzoneMin;
	    
	    [Tooltip("")]
	    public float radialMax = InputSystem.settings.defaultDeadzoneMax;
	    
	    [Tooltip("")]
	    public float crossMin = InputSystem.settings.defaultDeadzoneMin;
	    
	    [Tooltip("")]
	    public float crossMax = InputSystem.settings.defaultDeadzoneMax;

		public override Vector2 Process(Vector2 a_value, InputControl a_control)
		{
			Vector2 ret = CrossDeadzone(a_value, crossMin, crossMax);

			ret = RadialDeadzone(ret, radialMin, radialMax);

			return ret;
		}

		Vector2 CrossDeadzone(Vector2 a_input, float a_min, float a_max)
	    {
		    Vector2 ret = new Vector2(Mathf.Abs(a_input.x), Mathf.Abs(a_input.y));

		    float range = a_max - a_min;

		    ret.x = (ret.x - a_min) / range;
		    ret.y = (ret.y - a_min) / range;

			ret = Vector2.Max(ret, Vector2.zero);
			ret = Vector2.Min(ret, Vector2.one);

			ret.x *= Mathf.Sign(a_input.x);
		    ret.y *= Mathf.Sign(a_input.y);

			if (ret.sqrMagnitude > 1)
		    {
			    ret.Normalize();
		    }

		    return ret;
	    }
	    
	    Vector2 RadialDeadzone(Vector2 a_input, float a_min, float a_max)
	    {
		    float magnitude = a_input.magnitude;
		    
		    if (Math.Abs(magnitude) < 0.001f)
		    {
			    return Vector2.zero;
		    }

			float retLength = (magnitude - a_min) / (a_max - a_min);
			retLength = Mathf.Max(retLength, 0);
			retLength = Mathf.Min(retLength, 1);
			
		    Vector2 ret = a_input * (retLength / magnitude);
		    
		    return ret;
	    }

	#if UNITY_EDITOR
		static CrossRadialDeadzoneProcessor()
		{
			Initialize();
		}
	#endif

		[RuntimeInitializeOnLoadMethod]
		static void Initialize()
		{
			InputSystem.RegisterProcessor<CrossRadialDeadzoneProcessor>();
		}
	}
}