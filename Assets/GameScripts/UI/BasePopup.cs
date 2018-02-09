using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour {

	static GameObject canvas;
	static bool isPopupActive;

	bool isActive;

	public bool stopGame;

	virtual public void ShowPopup(bool stopGame = true) {
		BasePopup.isPopupActive = true;
		this.stopGame = stopGame;
		isActive = true;
		gameObject.SetActive (true);
		StartCoroutine (ShowNextFrame ());
	}
		
	public void HidePopup() {
		BasePopup.isPopupActive = false;
		isActive = false;
		gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	IEnumerator ShowNextFrame() {
		yield return new WaitForEndOfFrame ();
		if (isActive && stopGame) {
			Time.timeScale = 0;
		}
	}

	static public T GetPopup<T>() where T: BasePopup {
		if (BasePopup.canvas == null) {
			BasePopup.canvas = GameObject.Find ("HUDCanvas");
		}

		if (BasePopup.canvas != null) {
			return BasePopup.canvas.GetComponentInChildren<T> (true);
		} else {
			return null;
		}

	}

	static public bool IsPopupActive() {
		return isPopupActive;
	}
	
}
