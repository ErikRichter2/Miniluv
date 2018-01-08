using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour {

	public Entity EntityPrefab;

	private GameObject Checkpoint;

	// Use this for initialization
	void Start () {
		Checkpoint = GameObject.Find ("Checkpoint1");
		StartCoroutine (Generate ());
	}

	IEnumerator Generate() {
		while (true) {
			Entity obj = Instantiate<Entity> (EntityPrefab);
			obj.transform.parent = GameObject.Find ("EntityHolder").transform;
			obj.transform.position = transform.Find ("Place").transform.position;
			yield return new WaitForSeconds (Random.Range(0.5f, 4f));
			obj.MoveToCheckpoint (Checkpoint.transform.Find ("Queue").gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
