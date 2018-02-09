using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

[System.Serializable]
public class Rule {
	public int taskId;
	public List<int> stamps;
	public int collectedCount;
	public int StartDayCounter;

	public void AddStamp(int stampId) {
		this.stamps.Add (stampId);
		GameModel.GetModel<Rules>().Save ();
	}

	public void AddCollectedCount() {
		++this.collectedCount;
		GameModel.GetModel<Rules>().Save ();
	}

	public void RemoveAllStamps() {
		this.stamps.Clear ();
		GameModel.GetModel<Rules>().Save ();
	}

	public bool HasStamps() {
		return (this.stamps.Count > 0);
	}

	public bool HasStamp(int stampId) {
		return (this.stamps.IndexOf(stampId) != -1);
	}
}

[System.Serializable]
public class Rules : ScriptableObject, IModel, ISerializable {

	[SerializeField]
	List<Rule> rules;

	List<Rule> rulesForCurrentDay;

	public Rules() {
		this.rules = new List<Rule> ();
		this.rulesForCurrentDay = new List<Rule> ();

		// inital rules
		foreach (TaskDef task in DefinitionsLoader.taskDefinition.Items) {
			Rule rule = new Rule ();
			rule.taskId = task.Id;
			rule.stamps = new List<int> ();
			rule.StartDayCounter = 0;
			this.rules.Add (rule);
		}
	}

	public void SetRulesForDay(int CurrentDayId, int CurrentDayCounter) {
		DaysDef dayDef = DefinitionsLoader.daysDefinition.GetItem (CurrentDayId);

		// active for more days
		foreach (Rule rule in this.rules) {
			TaskDef taskDef = DefinitionsLoader.taskDefinition.GetItem (rule.taskId);
			if (rule.StartDayCounter > 0 && rule.StartDayCounter + taskDef.Duration >= CurrentDayCounter) {
				rulesForCurrentDay.Add (rule);
			} 
		}

		// started on this day
		DaysDef daysDef = DefinitionsLoader.daysDefinition.GetItem (CurrentDayId);
		foreach (int taskId in daysDef.StartTaskId) {
			Rule rule = GetRule (taskId);
			if (rule != null && rulesForCurrentDay.IndexOf(rule) == -1) {
				rulesForCurrentDay.Add (rule);
				rule.collectedCount = 0;
			}
		}
	}

	public List<Rule> GetRulesForCurrentDay() {
		return this.rulesForCurrentDay;
	}

	public Rule GetRule(int taskId) {
		foreach (Rule It in this.rules) {
			if (It.taskId == taskId) {
				return It;
			}
		}

		Assert.IsTrue (false);
		return null;
	}

	public Rule GetRandomRuleForCurrentDay() {
		Assert.IsTrue (this.rulesForCurrentDay.Count > 0);
		return this.rulesForCurrentDay [Mathf.FloorToInt(Random.Range(0, this.rulesForCurrentDay.Count))];
	}

	public List<StampDef> GetStamps(Rule rule, bool required) {

		List<StampDef> result = new List<StampDef> ();

		foreach (StampDef stamp in DefinitionsLoader.stampDefinition.Items) {
			bool isRequired = false;
			foreach (int stampId in rule.stamps) {
				if (stampId == stamp.Id) {
					isRequired = true;
					break;
				}
			}

			if (isRequired && required || !isRequired && !required) {
				result.Add (stamp);
			}
		}

		return result;
	}

	public void Load() {
		string savePath = Path.Combine (Application.persistentDataPath, "rules.txt");
		if (File.Exists (savePath)) {
			string saveData = File.ReadAllText(savePath);
			JsonUtility.FromJsonOverwrite(saveData, this);
		}
	}

	public void Save() {
		string savePath = Path.Combine (Application.persistentDataPath, "rules.txt");
		string saveData = JsonUtility.ToJson (this);
		File.WriteAllText (savePath, saveData);
	}

	public void DeleteSave() {
		File.Delete(Path.Combine (Application.persistentDataPath, "rules.txt"));
	}

	public void ClearAll() {
		foreach (Rule rule in this.rules) {
			rule.RemoveAllStamps ();
		}

		this.Save ();
	}

}
