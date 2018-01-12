using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

[System.Serializable]
public class Rule {
	public int taskId;
	public List<int> stamps;

	public void AddStamp(int stampId) {
		this.stamps.Add (stampId);
		Rules.Instance.Save ();
	}

	public void RemoveAllStamps() {
		this.stamps.Clear ();
		Rules.Instance.Save ();
	}

	public bool HasStamps() {
		return (this.stamps.Count > 0);
	}

	public bool HasStamp(int stampId) {
		return (this.stamps.IndexOf(stampId) != -1);
	}
}

[System.Serializable]
public class Rules : ScriptableObject {

	static public Rules Instance;

	[SerializeField]
	List<Rule> rules;

	public Rules() {
		this.rules = new List<Rule> ();

		// inital rules
		foreach (TaskDef task in DefinitionsLoader.taskDefinition.tasks) {
			Rule rule = new Rule ();
			rule.taskId = task.id;
			rule.stamps = new List<int> ();
			this.rules.Add (rule);
		}
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

	public List<Rule> GetRules() {
		return this.rules;
	}

	public Rule GetRandomRule() {
		Assert.IsTrue (this.rules.Count > 0);
		return this.rules [Mathf.FloorToInt(Random.Range(0, this.rules.Count))];
	}

	public List<StampDef> GetStamps(Rule rule, bool required) {

		List<StampDef> result = new List<StampDef> ();

		foreach (StampDef stamp in DefinitionsLoader.stampDefinition.stamps) {
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
		string savePath = Path.Combine (Application.streamingAssetsPath, "rules.txt");
		if (File.Exists (savePath)) {
			string saveData = File.ReadAllText(savePath);
			JsonUtility.FromJsonOverwrite(saveData, this);
		}
	}

	public void Save() {
		string savePath = Path.Combine (Application.streamingAssetsPath, "rules.txt");
		string saveData = JsonUtility.ToJson (this);
		File.WriteAllText (savePath, saveData);
	}


}
