using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProgressBar : MonoBehaviour {

	ITask task;
	Vector3 localScale;

	// Update is called once per frame
	void Update () {
		if (this.task != null) {
			localScale.x = Mathf.Clamp (task.GetRemainingTime () / task.GetDuration (), 0f, 1f);
			GetComponent<RectTransform> ().localScale = localScale;
		}
	}

	public void SetTask(ITask task) {
		gameObject.SetActive (true);
		this.task = task;
		localScale = new Vector3 (1f, 1f, 1f);
		GetComponent<RectTransform> ().localScale = localScale;
	}

	public void StopTask() {
		gameObject.SetActive (false);
		this.task = null;
	}

}
