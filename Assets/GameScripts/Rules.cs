using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Rule {
	public int RuleId;
	public int RuleType;
	public List<int> RuleColors;
}

struct RuleTypeDef {
	public int Id;
	public string Type;

	public RuleTypeDef(int p1, string p2) {
		this.Id = p1;
		this.Type = p2;
	}
}

public struct RuleColorDef {
	public int Id;
	public Color Color;

	public RuleColorDef(int p1, Color p2) {
		this.Id = p1;
		this.Color = p2;
	}
}

public class Rules : MonoBehaviour {

	static int RULE_ID_COUNTER;

	static List<RuleTypeDef> RuleTypes;
	static public List<RuleColorDef> RuleColors;
	static List<Rule> RulesInstances;

	// Use this for initialization
	void Start () {
		RuleTypes = new List<RuleTypeDef> ();
		RuleColors = new List<RuleColorDef> ();
		RulesInstances = new List<Rule> ();

		RuleColors.Add (new RuleColorDef (1, Color.green));
		RuleColors.Add (new RuleColorDef (2, Color.red));
		RuleColors.Add (new RuleColorDef (3, Color.blue));
		RuleColors.Add (new RuleColorDef (4, Color.yellow));
		RuleColors.Add (new RuleColorDef (5, Color.cyan));

		RuleTypes.Add (new RuleTypeDef (1, "WEDDING"));
		RuleTypes.Add (new RuleTypeDef (2, "DIVORCE"));
		RuleTypes.Add (new RuleTypeDef (3, "PASSPORT"));
		RuleTypes.Add (new RuleTypeDef (4, "FUNERAL"));

		foreach (RuleTypeDef RuleType in RuleTypes) {
			Rule RuleInstance = new Rule ();
			RuleInstance.RuleId = ++RULE_ID_COUNTER;
			RuleInstance.RuleType = RuleType.Id;
			RuleInstance.RuleColors = new List<int> ();
			Rules.RulesInstances.Add (RuleInstance);
		}
	}

	public static Rule GetRule(int RuleType) {
		foreach (Rule It in Rules.RulesInstances) {
			if (It.RuleType == RuleType) {
				return It;
			}
		}

		return RulesInstances[0];
	}

	static public void AddColor(int RuleType, int RuleColor) {
		Rules.GetRule (RuleType).RuleColors.Add (RuleColor);
	}

	static public void RemoveColor(int RuleType, int RuleColor) {
		Rules.GetRule (RuleType).RuleColors.Remove (RuleColor);
	}

	static public string GetType(int TypeId) {
		foreach (RuleTypeDef It in Rules.RuleTypes) {
			if (TypeId == It.Id) {
				return It.Type;
			}
		}

		return null;
	}

	static public Color GetColor(int ColorId) {
		foreach (RuleColorDef It in Rules.RuleColors) {
			if (ColorId == It.Id) {
				return It.Color;
			}
		}

		return Color.black;
	}

	static public Rule GetRandomRule() {
		int Index = Mathf.FloorToInt(Random.Range(0, Rules.RulesInstances.Count));
		return Rules.RulesInstances [Index];
	}

}
