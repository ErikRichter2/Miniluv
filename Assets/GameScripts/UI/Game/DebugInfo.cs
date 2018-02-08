using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour {

	float secondRefresh = 0;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Preloader.Loaded) {
			if (secondRefresh >= 1) {
				text.text = GetInfo ();
				secondRefresh = 0;
			} else {
				secondRefresh += Time.deltaTime;
			}
		}
	}


	string GetInfo() {
		string res = "";

		string dayInfo = DefinitionsLoader.daysDefinition.GetDebugInfo (GameModel.GetModel<Customers> ().CurrentDayId);

		res += "DAY: " + dayInfo;

		return res;
	}
}
