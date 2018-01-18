using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtils : MonoBehaviour {

	public static T GetComponentInChildren<T>(GameObject current, bool recursive = true) {
		T result = default(T);

		for (int i = 0; i < current.transform.childCount; ++i) {
			Transform child = current.transform.GetChild (i);
			result = child.GetComponent<T> ();
			if (result != null) {
				return result;
			}

			if (recursive) {
				result = GetComponentInChildren<T> (child.gameObject, recursive);
				if (result != null) {
					return result;
				}
			}
		}

		return result;
	}

	public static T GetComponentInParent<T>(GameObject current, bool recursive = true) {
		Transform parentTransform = current.transform.parent;

		while(parentTransform != null) {
			T component = parentTransform.GetComponent<T> ();
			if (component != null) {
				return component;
			}

			if (recursive) {
				parentTransform = parentTransform.parent;
			} else {
				break;
			}
		}

		return default(T);
	}


}
