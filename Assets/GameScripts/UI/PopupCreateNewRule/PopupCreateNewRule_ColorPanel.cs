using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnColorClickedHandler(PopupCreateNewRule_ColorPanel panel, RuleColorDef ruleColor);

public class PopupCreateNewRule_ColorPanel : MonoBehaviour {

	public GameObject RuleColorPrefab;
	public OnColorClickedHandler ClickHandler;

	public void Clear() {
		foreach (Transform Child in transform) {
			GameObject.Destroy(Child.gameObject);
		}
	}

	public void Add(RuleColorDef RuleColor) {
		GameObject NewRuleColor = Instantiate<GameObject> (RuleColorPrefab, transform);
		NewRuleColor.GetComponent<PopupCreateNewRule_ColorItem>().SetRuleColor (RuleColor);
	}

	public void Remove(RuleColorDef RuleColor) {
		foreach (PopupCreateNewRule_ColorItem RuleColorItem in GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
			if (RuleColorItem.RuleColor.Id == RuleColor.Id) {
				GameObject.Destroy (RuleColorItem.gameObject);
				break;
			}
		}
	}

	public void OnColorClicked(RuleColorDef RuleColor) {
		this.ClickHandler(this, RuleColor);
	}

}
