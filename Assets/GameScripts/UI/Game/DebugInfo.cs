using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour {

	float secondRefresh = 1;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Preloader.Loaded) {
			if (secondRefresh >= 1) {
				text.text = GetInfo ();
				secondRefresh = 0;
			} else {
				secondRefresh += Time.deltaTime;
			}
		}
	}


	string GetInfo() {
		string res = "";

		// current day
		string dayInfo = DefinitionsLoader.daysDefinition.GetDebugInfo (GameModel.GetModel<Customers> ().CurrentDayId);
		res += "DAY: " + dayInfo;

		// current rules
		res += "\nRULES: ";
		List<Rule> rules = GameModel.GetModel<Rules>().GetRulesForCurrentDay();
		foreach (Rule rule in rules) {
			res += "T: " + rule.taskId.ToString () + ",Cnt: " + rule.collectedCount + " ; ";
		}

		// finish conditions
		res += "\nNEXT DAYS: ";
		Customers customers = GameModel.GetModel<Customers> ();
		Rules rulesModel = GameModel.GetModel<Rules> ();

		foreach (int nextDayId in DefinitionsLoader.daysDefinition.GetItem(customers.CurrentDayId).Next) {
			res += "\n    " + nextDayId;
			DaysDef nextDay = DefinitionsLoader.daysDefinition.GetItem (nextDayId);

			if (nextDay.ReqTasksOK.Length > 0) {
				res += "\n        OK: ";
				foreach (int taskId in nextDay.ReqTasksOK) {
					res += "T:" + taskId + ",Cnt:" + rulesModel.GetRule(taskId).collectedCount + ",Cond:" + DefinitionsLoader.taskDefinition.GetItem(taskId).Condition + " ; ";
				}
			}

			if (nextDay.ReqTasksNOK.Length > 0) {
				res += "\n        NOK: ";
				foreach (int taskId in nextDay.ReqTasksNOK) {
					res += "T:" + taskId + ",Cnt:" + rulesModel.GetRule(taskId).collectedCount + ",Cond:" + DefinitionsLoader.taskDefinition.GetItem(taskId).Condition + " ; ";
				}
			}

			if (nextDay.ReqTasksOK_OR.Length > 0) {
				res += "\n        OR_OK: ";
				foreach (int taskId in nextDay.ReqTasksOK_OR) {
					res += "T:" + taskId + ",Cnt:" + rulesModel.GetRule(taskId).collectedCount + ",Cond:" + DefinitionsLoader.taskDefinition.GetItem(taskId).Condition + " ; ";
				}
			}

			if (nextDay.ReqTasksNOK_OR.Length > 0) {
				res += "\n        OR_NOK: ";
				foreach (int taskId in nextDay.ReqTasksNOK_OR) {
					res += "T:" + taskId + ",Cnt:" + rulesModel.GetRule(taskId).collectedCount + ",Cond:" + DefinitionsLoader.taskDefinition.GetItem(taskId).Condition + " ; ";
				}
			}
		}

		return res;
	}
}
