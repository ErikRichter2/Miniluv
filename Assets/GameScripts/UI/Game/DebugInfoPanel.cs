using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfoPanel : MonoBehaviour {


	private bool IsShow;

	public void ToggleShow() {
		if (IsShow) {
			GetComponent<RectTransform> ().position = new Vector3 (-700, GetComponent<RectTransform> ().position.y, 0);
		} else {
			GetComponent<RectTransform> ().position = new Vector3 (-441, GetComponent<RectTransform> ().position.y, 0);
		}

		IsShow = !IsShow;
	}

}
