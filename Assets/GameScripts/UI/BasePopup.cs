using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour {

	static GameObject canvas;

	bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;
		gameObject.SetActive (false);
	}

	public void ShowPopup() {
		isActive = true;
		gameObject.SetActive (true);
		StartCoroutine (ShowNextFrame ());
	}
		
	public void HidePopup() {
		isActive = false;
		gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	IEnumerator ShowNextFrame() {
		yield return new WaitForEndOfFrame ();
		if (isActive) {
			Time.timeScale = 0;
		}
	}

	static public T GetPopup<T>() where T: BasePopup {
		if (BasePopup.canvas == null) {
			BasePopup.canvas = GameObject.Find ("Canvas");
		}

		return BasePopup.canvas.GetComponentInChildren<T> (true);
	}
	
}
