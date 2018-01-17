using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Customer {
	public int defId;
	public int instanceId;
	public int taskId;
	public List<int> collectedStamps;
	public bool wasInfo;

	public Customer() {
		this.collectedStamps = new List<int> ();
	}
}

[System.Serializable]
public class Customers : ScriptableObject {

	public List<Customer> customers;

	public Customers() {
		this.customers = new List<Customer> ();
	}

	public void DeleteSave() {
		File.Delete(Path.Combine (Application.persistentDataPath, "customers.txt"));
	}

	public void Save() {
		string savePath = Path.Combine (Application.persistentDataPath, "customers.txt");
		string saveData = JsonUtility.ToJson (this);
		File.WriteAllText (savePath, saveData);
	}

	public void Load() {
		string savePath = Path.Combine (Application.persistentDataPath, "customers.txt");
		if (File.Exists (savePath)) {
			string saveData = File.ReadAllText(savePath);
			JsonUtility.FromJsonOverwrite(saveData, this);
		}
	}

	public Customer AddCustomer(int instanceId, int defId, int taskId) {
		Customer item = new Customer ();
		item.defId = defId;
		item.instanceId = instanceId;
		item.taskId = taskId;
		this.customers.Add (item);

		this.Save ();

		return item;
	}

	public void SetInfo(int instanceId, bool info) {
		this.GetCustomer (instanceId).wasInfo = info;
		this.Save ();
	}

	public void AddStamp(int instanceId, int stampId) {
		this.GetCustomer (instanceId).collectedStamps.Add (stampId);
		this.Save ();
	}

	public void RemoveCustomer(int instanceId) {
		this.customers.Remove (this.GetCustomer (instanceId));
		this.Save ();
	}

	Customer GetCustomer(int instanceId) {
		foreach (Customer It in this.customers) {
			if (It.instanceId == instanceId) {
				return It;
			}
		}

		return null;
	}
}
