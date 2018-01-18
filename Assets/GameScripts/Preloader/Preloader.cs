using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad(gameObject);
		StartCoroutine (this.Load ());
	}
	
	IEnumerator Load () {

		// create singletons
		new Logger ();
		new GameModel();

		// load game definitions
		yield return StartCoroutine (GetComponent<DefinitionsLoader>().LoadDefinitions());

		// load game model
		GameModel.Instance.Init ();
		GameModel.Instance.Load ();
		Debug.Log("GAME PROGRESS loaded");

		// load scene
		SceneManager.LoadScene ("Main");
	}
}
