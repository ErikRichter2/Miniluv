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


public class Rules : MonoBehaviour {

	static int RULE_ID_COUNTER;

	static List<RuleTypeDef> RuleTypes;
	//static public List<RuleColorDef> RuleColors;
	static public List<Rule> RulesInstances;

	// Use this for initialization
	void Start () {
		RuleTypes = new List<RuleTypeDef> ();
		RulesInstances = new List<Rule> ();

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

	static public void AddColor(int RuleType, int StamDefId) {
		Rules.GetRule (RuleType).RuleColors.Add (StamDefId);
	}

	static public void RemoveColor(int RuleType, int StamDefId) {
		Rules.GetRule (RuleType).RuleColors.Remove (StamDefId);
	}

	static public string GetType(int TypeId) {
		foreach (RuleTypeDef It in Rules.RuleTypes) {
			if (TypeId == It.Id) {
				return It.Type;
			}
		}

		return null;
	}
	/*
	static public Color GetColor(int ColorId) {
		foreach (RuleColorDef It in Rules.RuleColors) {
			if (ColorId == It.Id) {
				return It.Color;
			}
		}

		return Color.black;
	}
*/
	static public Rule GetRandomRule() {
		int Index = Mathf.FloorToInt(Random.Range(0, Rules.RulesInstances.Count));
		return Rules.RulesInstances [Index];
	}

	static public List<StampDef> GetStamps(Rule ruleInstance, bool required) {

		List<StampDef> result = new List<StampDef> ();

		foreach (StampDef stamp in DefinitionsLoader.stampDefinition.stamps) {
			bool isRequired = false;
			foreach (int ruleColorId in ruleInstance.RuleColors) {
				if (ruleColorId == stamp.Id) {
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


}
