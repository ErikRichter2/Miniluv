using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		this.text = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (Preloader.Loaded) {
			this.text.text = (GameModel.GetModel<Customers> ().GetDayLength() - Mathf.RoundToInt(GameModel.GetModel<Customers> ().CurrentTime)).ToString();
		}
	}
}
