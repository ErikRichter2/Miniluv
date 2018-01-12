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

		this.RuleName.text = Rules.GetType (rule.RuleType);

		List<RuleColorDef> ruleColors;

		ruleColors = Rules.GetColors (rule, false);
		foreach (RuleColorDef ruleColor in ruleColors) {
			this.PanelAvailable.Add (ruleColor);
		}

		ruleColors = Rules.GetColors (rule, true);
		foreach (RuleColorDef ruleColor in ruleColors) {
			this.PanelRequired.Add (ruleColor);
		}
	}

	void Clear() {
		this.PanelAvailable.Clear ();
		this.PanelRequired.Clear ();
	}

	public void OnRuleClicked(PopupCreateNewRule_ColorPanel Panel, RuleColorDef Rule) {
		if (Panel == this.PanelAvailable) {
			this.PanelAvailable.Remove (Rule);
			this.PanelRequired.Add (Rule);
		} else {
			this.PanelAvailable.Add (Rule);
			this.PanelRequired.Remove (Rule);
		}
	}


}
