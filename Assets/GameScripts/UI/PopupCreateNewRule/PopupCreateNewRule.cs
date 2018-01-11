using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreateNewRule : MonoBehaviour {

	public PopupCreateNewRule_ColorPanel PanelAvailabale;
	public PopupCreateNewRule_ColorPanel PanelRequired;
	public Text RuleName;

	Rule Rule;

	public void ShowRule(Rule Rule) {

		this.Rule = Rule;

		PanelAvailabale.Clear ();
		PanelRequired.Clear();

		RuleName.text = Rules.GetType (Rule.RuleType);

		foreach (RuleColorDef ColorDef in Rules.RuleColors) {
			bool IsRequired = false;
			foreach (int RuleColorId in Rule.RuleColors) {
				if (RuleColorId == ColorDef.Id) {
					IsRequired = true;
					break;
				}
			}

			if (IsRequired) {
				PanelRequired.Add (ColorDef);
			} else {
				PanelAvailabale.Add (ColorDef);
			}
		}
	}

	public void OnRuleClicked(PopupCreateNewRule_ColorPanel Panel, RuleColorDef Rule) {
		if (Panel == PanelAvailabale) {
			PanelAvailabale.Remove (Rule);
			PanelRequired.Add (Rule);
		} else {
			PanelAvailabale.Add (Rule);
			PanelRequired.Remove (Rule);
		}
	}

	public void OnConfirm() {
		this.Rule.RuleColors.Clear ();

		foreach (PopupCreateNewRule_ColorItem ColorItem in PanelRequired.GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
			Rules.AddColor (this.Rule.RuleType, ColorItem.RuleColor.Id);
		}

		gameObject.SetActive (false);
	}

}
