using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupShowRules : BasePopup {

	public GameObject RuleItemPrefab;
	public GameObject Content;

	public void ShowStamp(int stampId) {
		
		this.Clear ();

		foreach (Rule rule in GameModel.Instance.Rules.GetRules()) {
			if (stampId == 0 || rule.HasStamp (stampId)) {
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
		foreach (PopupShowRules_RuleItem ruleItem in ruleItems) {
			ruleItem.rule.RemoveAllStamps ();
			foreach (PopupCreateNewRule_ColorItem colorItem in ruleItem.PanelRequired.GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
				ruleItem.rule.AddStamp (colorItem.stamp.Id);
			}
		}

		HidePopup ();
	}

}
