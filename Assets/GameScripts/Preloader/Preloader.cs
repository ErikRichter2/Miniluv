using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour {

	public static bool Loaded;

	void Start () {
		DontDestroyOnLoad(gameObject);
		Application.logMessageReceived += this.HandleLog;
		StartCoroutine (this.Load ());
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		if (type == LogType.Error || type == LogType.Exception) {
			BasePopup.GetPopup<PopupLogger> ().ShowMessage (logString + "\n\n" + stackTrace);
		}

		if (Preloader.Loaded == false && type == LogType.Log) {
			Text text = GameObjectUtils.GetComponentInChildren<Text> (gameObject);
			text.text += "\n" + logString;
		}
	}
	
	IEnumerator Load () {

		// unload Main scene
		yield return SceneManager.UnloadSceneAsync("Main");

		// load game definitions
		yield return StartCoroutine (GetComponent<DefinitionsLoader>().LoadDefinitions());

		// load game model
		new GameModel();
		GameModel.Instance.Init ();
		GameModel.Instance.Load ();
		Debug.Log("GAME PROGRESS loaded");

		// load scene
		GetComponentInChildren<Canvas>().gameObject.SetActive(false);
		Preloader.Loaded = true;
		SceneManager.LoadScene ("Main");
	}
}
