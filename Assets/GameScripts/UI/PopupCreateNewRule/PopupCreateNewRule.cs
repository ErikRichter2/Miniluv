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

		this.ruleName.text = DefinitionsLoader.taskDefinition.GetItem(this.rule.taskId).Name;

		List<StampDef> stamps;

		stamps = GameModel.GetModel<Rules>().GetStamps (rule, false);
		foreach (StampDef stamp in stamps) {
			this.panelAvailabale.Add (stamp);
		}

		stamps = GameModel.GetModel<Rules>().GetStamps (rule, true);
		foreach (StampDef stamp in stamps) {
			this.panelRequired.Add (stamp);
		}

		ShowPopup ();
	}

	public void OnRuleClicked(PopupCreateNewRule_ColorPanel Panel, StampDef stamp) {
		if (Panel == this.panelAvailabale) {
			this.panelAvailabale.Remove (stamp);
			this.panelRequired.Add (stamp);
		} else {
			this.panelAvailabale.Add (stamp);
			this.panelRequired.Remove (stamp);
		}
	}

	public void OnConfirm() {
		this.rule.RemoveAllStamps ();

		foreach (PopupCreateNewRule_ColorItem colorItem in this.panelRequired.GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
			this.rule.AddStamp (colorItem.stamp.Id);
		}

		HidePopup ();
	}

}
