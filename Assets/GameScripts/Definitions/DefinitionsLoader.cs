using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DefinitionsLoader : MonoBehaviour {

	const string STAMPS_DEF = "https://pastebin.com/raw/ZexpweWC";
	static public StampDefiniton stampDefinition;

	static public TaskDefinition taskDefinition;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		StartCoroutine(LoadDefinitions ());
	}

	IEnumerator LoadDefinitions() {
		WWW www;

		// load game definitions	

		// STAMPS
		stampDefinition = new StampDefiniton ();
		www = new WWW (STAMPS_DEF);
		yield return www;
		stampDefinition.Parse (www.text);

		// TASKS
		taskDefinition = new TaskDefinition ();

		// load game progress
		Rules.Instance = ScriptableObject.CreateInstance<Rules>();
		Rules.Instance.Load ();

		// init game
		Debug.Log("Definitions loaded");
		SceneManager.LoadScene ("Main");
	}
		

}
