using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject CheckpointsGameObject;
	public InfoPoint InfoPoint;

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
			Entity Entity = Instantiate<Entity> (this.Entities[Index], transform.Find ("Place"));
				
			// random delay
			yield return new WaitForSeconds (Random.Range(0.25f, 2f));

			// add checkpoints and exit point
			foreach(Checkpoint Checkpoint in this.Checkpoints) {
				Entity.GetComponent<CustomerBehaviour>().AddCheckpoint(Checkpoint);
			}

			Entity.GetComponent<CustomerBehaviour> ().SetInfoPoint (InfoPoint);
			Entity.GetComponent<CustomerBehaviour> ().SetExitPoint (transform.Find ("Place").gameObject);
			Entity.GetComponent<CustomerBehaviour> ().SetRule (Rules.GetRandomRule());

			// find and goto checkpoint
			Entity.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFOPOINT);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
