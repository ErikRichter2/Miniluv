using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefinitionsLoader : MonoBehaviour {

	const string STAMPS_DEF = "https://pastebin.com/raw/ZexpweWC";

	static public StampDefiniton stampDefinition;

	IEnumerator LoadDefinitions() {
		WWW www;

		// STAMP
		stampDefinition = new StampDefiniton ();
		www = new WWW (STAMPS_DEF);
		yield return www;
		stampDefinition.Parse (www.text);

		// all loaded
		Debug.Log("Definitions loaded");
		SceneManager.LoadScene ("Main");
	}
		
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		StartCoroutine(LoadDefinitions ());
	}


}
