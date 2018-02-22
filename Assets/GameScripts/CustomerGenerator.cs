using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour {

	public GameObject stampDesksGameObjects;
	public InfoDesk infoDesk;
	public Entity CustomerPrefab;
	private bool CanGenerate;
	private bool AllCustomersGenerated;
	private float NextCustomerDelay;

	private StampDesk[] stampDesks;
	private List<CustomerBehaviour> customers;

	public void Init() {
		this.customers = new List<CustomerBehaviour> ();
		this.CanGenerate = true;
		this.AllCustomersGenerated = false;
		this.stampDesks = this.stampDesksGameObjects.GetComponentsInChildren<StampDesk> ();
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

				if (BasePopup.IsPopupActive() == false) {
					PopupNextDay popup = BasePopup.GetPopup<PopupNextDay>();
					popup.ShowPopup (false);
				}

				this.CanGenerate = false;
				this.AllCustomersGenerated = false;
				while (this.customers.Count > 0) {
					CustomerBehaviour customer = this.customers [0];
					this.customers.Remove (customer);
					this.DestroyCustomer (customer);
				}

			}

			if (this.CanGenerate) {
				if (this.AllCustomersGenerated == false && NextCustomerDelay <= 0) {

					// create new customer with random visual
					List<CustomerDef> customers = DefinitionsLoader.customerDefinition.Items;
					Entity customer = this.CreateCustomer (customers [Mathf.FloorToInt (Random.Range (0, customers.Count))], 0);

					int taskId = GameModel.GetModel<Customers> ().GetTaskForNewCustomer ();
					customer.GetComponent<CustomerBehaviour> ().model = GameModel.GetModel<Customers> ().AddCustomer (customer.instanceId, customer.defId, taskId);
					customer.GetComponent<CustomerBehaviour> ().SetState (CustomerBehaviour.STATES.STATE_MOVE_TO_INFODESK);

					float delayForNextCustomer = GameModel.GetModel<Customers> ().GetDelayForNextCustomer ();
					if (delayForNextCustomer <= 0) {
						this.AllCustomersGenerated = true;
					} else {
						this.AllCustomersGenerated = false;
						this.NextCustomerDelay = Random.Range (delayForNextCustomer - delayForNextCustomer * 0.10f, delayForNextCustomer + delayForNextCustomer * 0.10f);
					}						
				} else {					
					this.NextCustomerDelay -= Time.deltaTime;
				}
			} else {
				if (GameModel.GetModel<Customers> ().IsDayFinished () == false) {
					this.CanGenerate = true;
				}
			}
		}
	}

	public void DestroyCustomer(CustomerBehaviour customer) {
		this.customers.Remove (customer);
		customer.DestroyCustomer ();
		DestroyObject (customer.gameObject);
	}


}
