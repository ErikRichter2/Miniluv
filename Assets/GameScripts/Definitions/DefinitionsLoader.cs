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

	public IEnumerator LoadDefinitions() {
		stampDefinition = new StampDefiniton ();
		yield return StartCoroutine (this.Load ("STAMPS", STAMPS_URL, stampDefinition));

		taskDefinition = new TaskDefinition ();
		yield return StartCoroutine (this.Load ("TASKS", TASKS_URL, taskDefinition));

		customerDefinition = new CustomerDefinition ();
		yield return StartCoroutine (this.Load ("CUSTOMERS", CUSTOMERS_URL, customerDefinition));

		configDefinition = new ConfigDefinition ();
		yield return StartCoroutine (this.Load ("CONFIG", CONFIG_URL, configDefinition));
	}

	IEnumerator Load(string name, string url, IDefinition definition) {

		WWW www;
		string localFile = Path.Combine (Application.persistentDataPath, "def_" + name + ".txt");
		string finalUrl = url + "/?rnd=" + Random.Range(0, 1000000).ToString();

		// load from local cache
		if (File.Exists (localFile)) {
			Debug.Log (name + " loaded from cache ");
			definition.Parse (File.ReadAllText(localFile));
		}

		// try load from network
		www = new WWW (finalUrl);
		yield return www;
		if (www.isDone && www.error == null) {
			Debug.Log (name + " loaded ");
			definition.Parse (www.text);

			// save to local file
			File.WriteAllText(localFile, www.text);
		} else {
			Debug.Log(name + " failed: " + www.error);
		}
	}
		

}
