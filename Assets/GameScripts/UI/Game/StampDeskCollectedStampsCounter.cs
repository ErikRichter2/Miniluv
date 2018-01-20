using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampDeskCollectedStampsCounter : MonoBehaviour {

	Stamp stamp;
	int currentValue;

	public void SetStampDesk(Stamp stamp) {
		this.stamp = stamp;
		this.currentValue = -1;
	}

	// Update is called once per frame
	void Update () {
		if (this.stamp != null) {
			int nextValue = this.stamp.collectedCount;
			if (this.currentValue != nextValue) {
				this.currentValue = nextValue;
				GetComponent<Text> ().text = this.currentValue + "x";
			}
		}
	}
}
