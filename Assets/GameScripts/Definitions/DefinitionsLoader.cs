using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DefinitionsLoader : MonoBehaviour {

	const string STAMPS_URL = "https://pastebin.com/raw/ZexpweWC";
	const string TASKS_URL = "https://pastebin.com/raw/H7AdzkW1";
	const string CUSTOMERS_URL = "https://pastebin.com/raw/dwrmxftT";
	const string CONFIG_URL = "https://pastebin.com/raw/GaXQbdfd";

	static public StampDefiniton stampDefinition;
	static public TaskDefinition taskDefinition;
	static public CustomerDefinition customerDefinition;
	static public ConfigDefinition configDefinition;

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
		www = new WWW (STAMPS_URL + "/?rnd=" + Random.Range(0, 1000000).ToString());
		yield return www;
		Debug.Log("STAMPS loaded " + www.error);
		stampDefinition.Parse (www.text);

		// TASKS
		taskDefinition = new TaskDefinition ();
		www = new WWW (TASKS_URL + "/?rnd=" + Random.Range(0, 1000000).ToString());
		yield return www;
		Debug.Log("TASKS loaded " + www.error);
		taskDefinition.Parse (www.text);

		// CUSTOMERS
		customerDefinition = new CustomerDefinition ();
		www = new WWW (CUSTOMERS_URL + "/?rnd=" + Random.Range(0, 1000000).ToString());
		yield return www;
		Debug.Log("CUSTOMERS loaded " + www.error);
		customerDefinition.Parse (www.text);

		// CONFIG
		configDefinition = new ConfigDefinition ();
		www = new WWW (CONFIG_URL + "/?rnd=" + Random.Range(0, 1000000).ToString());
		yield return www;
		Debug.Log("CONFIG loaded " + www.error);
		configDefinition.Parse (www.text);

		// load game progress
		Rules.Instance = ScriptableObject.CreateInstance<Rules>();
		Rules.Instance.Load ();
		Customers.Instance = ScriptableObject.CreateInstance<Customers> ();
		Customers.Instance.Load ();

		// init game
		SceneManager.LoadScene ("Main");
	}
		

}
