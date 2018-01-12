using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject CheckpointsGameObject;
	public InfoPoint InfoPoint;
	public Entity CustomerPrefab;

	private Checkpoint[] Checkpoints;

	// Use this for initialization
	void Start () {
		this.Checkpoints = this.CheckpointsGameObject.GetComponentsInChildren<Checkpoint> ();
		StartCoroutine (Generate ());
	}

	IEnumerator Generate() {
		while (true) {

			// create new customer
			Entity customer = Instantiate<Entity> (this.CustomerPrefab, transform.Find ("Place"));

			// set random def
			List<CustomerDef> customers = DefinitionsLoader.customerDefinition.Items;
			customer.SetEntityDef (customers [Mathf.FloorToInt (Random.Range (0, customers.Count))]);
				
			// random delay
			int customersPerMinute = int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.CUSTOMERS_PER_MINUTE).Value);
			float deltaSeconds = 60.0f / customersPerMinute;
			yield return new WaitForSeconds (Random.Range(deltaSeconds - deltaSeconds * 0.10f, deltaSeconds + deltaSeconds * 0.10f));

			// add checkpoints and exit point
			foreach(Checkpoint Checkpoint in this.Checkpoints) {
				customer.GetComponent<CustomerBehaviour>().AddCheckpoint(Checkpoint);
			}

			customer.GetComponent<CustomerBehaviour> ().SetInfoPoint (InfoPoint);
			customer.GetComponent<CustomerBehaviour> ().SetExitPoint (transform.Find ("Place").gameObject);
			customer.GetComponent<CustomerBehaviour> ().SetRule (Rules.Instance.GetRandomRule());

			// find and goto checkpoint
			customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFOPOINT);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
