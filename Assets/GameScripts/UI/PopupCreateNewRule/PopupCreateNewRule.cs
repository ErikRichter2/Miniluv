using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreateNewRule : BasePopup {

	public PopupCreateNewRule_ColorPanel panelAvailabale;
	public PopupCreateNewRule_ColorPanel panelRequired;
	public Text ruleName;

	Rule rule;

	public void ShowRule(Rule rule) {

		this.rule = rule;

		this.panelRequired.ClickHandler = OnRuleClicked;
		this.panelAvailabale.ClickHandler = OnRuleClicked;

		this.panelAvailabale.Clear ();
		this.panelRequired.Clear();

		this.ruleName.text = Rules.GetType (this.rule.RuleType);

		List<RuleColorDef> ruleColors;

		ruleColors = Rules.GetColors (rule, false);
		foreach (RuleColorDef ruleColor in ruleColors) {
			this.panelAvailabale.Add (ruleColor);
		}

		ruleColors = Rules.GetColors (rule, true);
		foreach (RuleColorDef ruleColor in ruleColors) {
			this.panelRequired.Add (ruleColor);
		}

		ShowPopup ();
	}

	public void OnRuleClicked(PopupCreateNewRule_ColorPanel Panel, RuleColorDef Rule) {
		if (Panel == this.panelAvailabale) {
			this.panelAvailabale.Remove (Rule);
			this.panelRequired.Add (Rule);
		} else {
			this.panelAvailabale.Add (Rule);
			this.panelRequired.Remove (Rule);
		}
	}

	public void OnConfirm() {
		this.rule.RuleColors.Clear ();

		foreach (PopupCreateNewRule_ColorItem colorItem in this.panelRequired.GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
			Rules.AddColor (this.rule.RuleType, colorItem.RuleColor.Id);
		}

		HidePopup ();
	}

}
