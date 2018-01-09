using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject CheckpointsGameObject;

	private Checkpoint[] Checkpoints;
	public Entity[] Entities;

	// Use this for initialization
	void Start () {
		this.Checkpoints = this.CheckpointsGameObject.GetComponentsInChildren<Checkpoint> ();
		StartCoroutine (Generate ());
	}

	IEnumerator Generate() {
		while (true) {

			// create new entity
			int Index = Mathf.FloorToInt(Random.Range(0, this.Entities.Length));
			Entity Entity = Instantiate<Entity> (this.Entities[Index]);
			Entity.transform.parent = GameObject.Find ("EntityHolder").transform;
			Entity.transform.position = transform.Find ("Place").transform.position;
				
			// random delay
			yield return new WaitForSeconds (Random.Range(0.5f, 4f));

			// add checkpoints and exit point
			foreach(Checkpoint Checkpoint in this.Checkpoints) {
				Entity.GetComponent<CustomerBehaviour>().AddCheckpoint(Checkpoint);
			}

			Entity.GetComponent<CustomerBehaviour> ().SetExitPoint (transform.Find ("Place").gameObject);

			// find and goto checkpoint
			Entity.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
