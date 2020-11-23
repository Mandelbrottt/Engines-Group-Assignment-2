using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	static T s_instance;

	public static T Instance {
		get {
			if (s_instance != null)
				return s_instance;

			var list = FindObjectsOfType<T>();

			if (list.Length == 1)
				return s_instance = list[0];

			if (list.Length > 1) {
				Debug.LogError($"There should only be one \"{nameof(T)}\" in the scene!");
				return null;
			}

			var obj = new GameObject(nameof(T), typeof(T));
			return s_instance = obj.GetComponent<T>();
		}
	}

	protected virtual void Awake() {
		if (s_instance != null) {
			throw new Exception($"An object of type \"{nameof(T)} already exists!");
		}
	}
}
