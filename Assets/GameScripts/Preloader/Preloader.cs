using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour {

	public static bool Loaded;

	public Text Logs;

	void Start () {
		DontDestroyOnLoad(gameObject);
		Application.logMessageReceived += this.HandleLog;
		this.Logs.text = "";
		StartCoroutine (this.Load ());
	}

	void Update() {
		if (GameModel.Instance != null) {
			GameModel.Instance.Update (Time.deltaTime);
		}
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		if (type == LogType.Error || type == LogType.Exception) {
			BasePopup.GetPopup<PopupLogger> ().ShowMessage (logString + "\n\n" + stackTrace);
		}

		if (Preloader.Loaded == false && type == LogType.Log) {
			this.Logs.text += "\n" + logString;
		}
	}
	
	IEnumerator Load () {

		// unload Main scene
		if (SceneManager.GetSceneByName ("Main").isLoaded) {
			yield return SceneManager.UnloadSceneAsync("Main");
		}

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
