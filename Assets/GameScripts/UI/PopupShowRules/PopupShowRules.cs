using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupShowRules : BasePopup {

	public GameObject RuleItemPrefab;
	public GameObject Content;

	public void ShowColor(int color) {
		
		this.Clear ();

		foreach (Rule rule in Rules.RulesInstances) {
			if (color == 0 || rule.RuleColors.IndexOf (color) != -1) {
				GameObject ruleInstance = Instantiate<GameObject> (RuleItemPrefab, this.Content.transform);
				ruleInstance.GetComponent<PopupShowRules_RuleItem> ().SetRule (rule);
			}
		}

		this.ShowPopup ();
	}

	void Clear() {
		foreach (Transform It in this.Content.transform) {
			GameObject.Destroy (It.gameObject);
		}
	}

	public void OnConfirm() {

		PopupShowRules_RuleItem[] ruleItems = this.Content.GetComponentsInChildren<PopupShowRules_RuleItem> ();
		foreach (PopupShowRules_RuleItem RuleItem in ruleItems) {
			RuleItem.rule.RuleColors.Clear ();
			foreach (PopupCreateNewRule_ColorItem colorItem in RuleItem.PanelRequired.GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
				Rules.AddColor (RuleItem.rule.RuleType, colorItem.RuleColor.Id);
			}
		}

		HidePopup ();
	}

}
