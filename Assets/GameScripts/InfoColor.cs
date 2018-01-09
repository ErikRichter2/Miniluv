using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoColor : MonoBehaviour {

	public void SetColor(Color Color) {
		GetComponent<SpriteRenderer> ().color = Color;
		SetActive (true);
	}

	public Color GetColor() {
		return GetComponent<SpriteRenderer> ().color;
	}

	public void SetActive(bool value) {
		gameObject.SetActive (value);
	}


}
