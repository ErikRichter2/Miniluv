using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShowRules_RuleItem : MonoBehaviour {

	public Text RuleName;
	public PopupCreateNewRule_ColorPanel PanelAvailable;
	public PopupCreateNewRule_ColorPanel PanelRequired;
	public Rule rule;

	public void SetRule(Rule rule) {

		this.rule = rule;

		this.Clear ();

		this.PanelAvailable.ClickHandler = OnRuleClicked;
		this.PanelRequired.ClickHandler = OnRuleClicked;

		this.RuleName.text = DefinitionsLoader.taskDefinition.GetItem (rule.taskId).Name;

		List<StampDef> stamps;

		stamps = GameModel.GetModel<Rules>().GetStamps (rule, false);
		foreach (StampDef stamp in stamps) {
			this.PanelAvailable.Add (stamp);
		}

		stamps = GameModel.GetModel<Rules>().GetStamps (rule, true);
		foreach (StampDef stamp in stamps) {
			this.PanelRequired.Add (stamp);
		}
	}

	void Clear() {
		this.PanelAvailable.Clear ();
		this.PanelRequired.Clear ();
	}

	public void OnRuleClicked(PopupCreateNewRule_ColorPanel Panel, StampDef stamp) {
		if (Panel == this.PanelAvailable) {
			this.PanelAvailable.Remove (stamp);
			this.PanelRequired.Add (stamp);
		} else {
			this.PanelAvailable.Add (stamp);
			this.PanelRequired.Remove (stamp);
		}
	}


}
