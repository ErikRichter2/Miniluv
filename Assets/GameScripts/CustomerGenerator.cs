using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject stampDesksGameObjects;
	public InfoDesk infoDesk;
	public Entity CustomerPrefab;

	private StampDesk[] stampDesks;

	public void Init() {
		this.stampDesks = this.stampDesksGameObjects.GetComponentsInChildren<StampDesk> ();
		this.LoadFromSave ();
		StartCoroutine (Generate ());
	}

	void LoadFromSave() {
		//yield return new WaitForSeconds (0.25f);
		foreach (Customer It in GameModel.GetModel<Customers>().customers) {
			Entity customer = this.CreateCustomer (DefinitionsLoader.customerDefinition.GetItem(It.defId), 0);
			customer.GetComponent<CustomerBehaviour> ().model = It;
			if (It.wasInfo) {
				customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_STAMPDESK, true);
			} else {
				customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFODESK, true);
			}
		}
	}

	Entity CreateCustomer(CustomerDef customerDef, int instanceId) {
		Entity customer = Instantiate<Entity> (this.CustomerPrefab, transform.Find ("Place"));
		customer.SetEntityDef (customerDef, instanceId);
		customer.GetComponent<CustomerBehaviour> ().SceneStampDesks = this.stampDesks;
		customer.GetComponent<CustomerBehaviour> ().InfoDesk = infoDesk;
		customer.GetComponent<CustomerBehaviour> ().ExitPoint = transform.Find ("Place").gameObject;

		return customer;
	}

	IEnumerator Generate() {
		while (true) {

			// create new customer with random def
			List<CustomerDef> customers = DefinitionsLoader.customerDefinition.Items;
			Entity customer = this.CreateCustomer(customers [Mathf.FloorToInt (Random.Range (0, customers.Count))], 0);

			// random delay
			int customersPerMinute = int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.CUSTOMERS_PER_MINUTE).Value);
			float deltaSeconds = 60.0f / customersPerMinute;
			yield return new WaitForSeconds (Random.Range(deltaSeconds - deltaSeconds * 0.10f, deltaSeconds + deltaSeconds * 0.10f));

			// find and goto checkpoint
			customer.GetComponent<CustomerBehaviour> ().model = GameModel.GetModel<Customers>().AddCustomer (customer.instanceId, customer.defId, GameModel.GetModel<Rules>().GetRandomRule().taskId);
			customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFODESK);
		}
	}


}
