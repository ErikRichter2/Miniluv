using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTimeText : MonoBehaviour {

	void Start () {
		if (Preloader.Loaded) {
			ITaskable taskable = GameObjectUtils.GetComponentInParent<ITaskable> (gameObject);
			if (taskable != null) {
				GetComponent<Text> ().text = Mathf.FloorToInt(taskable.GetTaskDuration() / 1000).ToString() + "s";
			}
		}
	}
		
}
