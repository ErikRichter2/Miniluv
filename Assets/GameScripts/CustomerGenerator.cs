using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject stampDesksGameObjects;
	public InfoDesk infoDesk;
	public Entity CustomerPrefab;
	private bool CanGenerate;
	private float NextCustomerDelay;

	private StampDesk[] stampDesks;
	private List<CustomerBehaviour> customers;

	public void Init() {
		this.customers = new List<CustomerBehaviour> ();
		this.CanGenerate = true;
		this.stampDesks = this.stampDesksGameObjects.GetComponentsInChildren<StampDesk> ();
		this.InitDay ();
		this.LoadFromSave ();
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
		this.customers.Add (customer.GetComponent<CustomerBehaviour> ());
		customer.GetComponent<CustomerBehaviour> ().CustomerGenerator = this;

		return customer;
	}

	void Update() {
		if (Preloader.Loaded) {
			if (GameModel.GetModel<Customers> ().IsDayFinished ()) {
				this.CanGenerate = false;
				while (this.customers.Count > 0) {
					CustomerBehaviour customer = this.customers [0];
					this.customers.Remove (customer);
					this.DestroyCustomer (customer);
				}

				GameModel.GetModel<Customers> ().StartNextDay ();
				this.InitDay ();
				this.CanGenerate = true;
			}

			if (this.CanGenerate) {

				if (NextCustomerDelay <= 0) {
					// create new customer with random def
					List<CustomerDef> customers = DefinitionsLoader.customerDefinition.Items;
					Entity customer = this.CreateCustomer (customers [Mathf.FloorToInt (Random.Range (0, customers.Count))], 0);

					// random delay
					int customersPerMinute = int.Parse (DefinitionsLoader.configDefinition.GetItem (ConfigDefinition.CUSTOMERS_PER_MINUTE).Value);
					float deltaSeconds = 60.0f / customersPerMinute;

					Rule rule = GameModel.GetModel<Rules> ().GetRandomRuleForCurrentDay ();
					customer.GetComponent<CustomerBehaviour> ().model = GameModel.GetModel<Customers> ().AddCustomer (customer.instanceId, customer.defId, rule.taskId);
					customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFODESK);

					this.NextCustomerDelay = Random.Range (deltaSeconds - deltaSeconds * 0.10f, deltaSeconds + deltaSeconds * 0.10f);
				} else {					
					this.NextCustomerDelay -= Time.deltaTime;
				}

			}
		}
	}

	public void DestroyCustomer(CustomerBehaviour customer) {
		this.customers.Remove (customer);
		customer.DestroyCustomer ();
		DestroyObject (customer.gameObject);
	}

	void InitDay() {
		Customers customers = GameModel.GetModel<Customers> ();
		GameModel.GetModel<Rules> ().SetRulesForDay (customers.CurrentDayId, customers.CurrentDayCounter);
	}


}
