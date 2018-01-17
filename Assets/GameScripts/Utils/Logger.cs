using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Application.logMessageReceived += this.HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		if (type == LogType.Error || type == LogType.Exception) {
			BasePopup.GetPopup<PopupLogger> ().ShowMessage (logString + "\n\n" + stackTrace);
		}
	}
}
