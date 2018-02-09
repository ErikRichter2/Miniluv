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

	public bool HasAllStampsCollected() {
		Rule rule = GameModel.GetModel<Rules> ().GetRule (this.taskId);
		return (rule.HasStamps() && GetFirstUncollectedStamp () == 0);
	}

	public int GetFirstUncollectedStamp() {
		foreach (int stampId in GameModel.GetModel<Rules>().GetRule(this.taskId).stamps) {
			if (this.collectedStamps.IndexOf (stampId) == -1) {
				return stampId;
			}
		}

		return 0;
	}
}

[System.Serializable]
public class Customers : ScriptableObject, IModel, ISerializable, ITickable {

	public List<Customer> customers;
	public int CurrentDayCounter;
	public int CurrentDayId;
	public float CurrentTime;

	private int DayLength = 0;

	public Customers() {
		this.customers = new List<Customer> ();
		this.CurrentTime = 0;
		this.CurrentDayId = 1;
		this.CurrentDayCounter = 1;
		this.DayLength = int.Parse (DefinitionsLoader.configDefinition.GetItem (ConfigDefinition.DAY_LENGTH).Value);
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

	public void UpdateModel(float delta) {
		this.CurrentTime += delta;
	}

	public int GetNextDayId() {
		bool result;
		int nextDayFinal = 0;

		Rules rules = GameModel.GetModel<Rules> ();
		DaysDef currentDay = DefinitionsLoader.daysDefinition.GetItem (this.CurrentDayId);
		foreach (int NextDayId in currentDay.Next) {
			DaysDef nextDay = DefinitionsLoader.daysDefinition.GetItem (NextDayId);

			// AND OK
			result = true;
			foreach (int taskId in nextDay.ReqTasksOK) {
				result = result && DefinitionsLoader.taskDefinition.GetItem (taskId).IsOK (rules.GetRule(taskId).collectedCount);
			}
			if (result == false) {
				continue;
			}

			// AND NOK
			result = true;
			foreach (int taskId in nextDay.ReqTasksOK) {
				result = result && !DefinitionsLoader.taskDefinition.GetItem (taskId).IsOK (rules.GetRule(taskId).collectedCount);
			}
			if (result == false) {
				continue;
			}

			// OR OK
			bool or_ok = true;
			if (nextDay.ReqTasksOK_OR.Length > 0) {
				or_ok = false;
				foreach (int taskId in nextDay.ReqTasksOK_OR) {
					result = result || DefinitionsLoader.taskDefinition.GetItem (taskId).IsOK (rules.GetRule(taskId).collectedCount);
				}
			}

			// OR NOK
			bool or_nok = true;
			if (nextDay.ReqTasksNOK_OR.Length > 0) {
				or_nok = false;
				foreach (int taskId in nextDay.ReqTasksNOK_OR) {
					result = result || !DefinitionsLoader.taskDefinition.GetItem (taskId).IsOK (rules.GetRule(taskId).collectedCount);
				}
			}

			if (!or_ok && !or_nok) {
				continue;
			}

			nextDayFinal = NextDayId;
			break;
		}

		return nextDayFinal;
	}

	public void StartNextDay() {

		int nextDayFinal = this.GetNextDayId ();
		// check for end
		int end = DefinitionsLoader.daysDefinition.GetItem (nextDayFinal).End;
		if (end != 0) {
			Debug.Log ("GAME END: " + end.ToString());
		} else {
			Debug.Log ("NEXT DAY: " + nextDayFinal.ToString());
			this.CurrentDayCounter++;
			this.CurrentDayId = nextDayFinal;
			this.CurrentTime = 0;
			this.customers.Clear ();
			this.Save ();
		}
	}

	public bool IsDayFinished() {
		return (this.CurrentTime >= this.DayLength);
	}

	public int GetDayLength() {
		return this.DayLength;
	}


}
