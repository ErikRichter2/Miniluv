using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnColorClickedHandler(PopupCreateNewRule_ColorPanel panel, StampDef stamp);

public class PopupCreateNewRule_ColorPanel : MonoBehaviour {

	public GameObject StampPrefab;
	public OnColorClickedHandler ClickHandler;

	public void Clear() {
		foreach (Transform Child in transform) {
			GameObject.Destroy(Child.gameObject);
		}
	}

	public void Add(StampDef stamp) {
		GameObject NewRuleColor = Instantiate<GameObject> (StampPrefab, transform);
		NewRuleColor.GetComponent<PopupCreateNewRule_ColorItem>().SetStampDef (stamp);
	}

	public void Remove(StampDef stamp) {
		foreach (PopupCreateNewRule_ColorItem RuleColorItem in GetComponentsInChildren<PopupCreateNewRule_ColorItem>()) {
			if (RuleColorItem.stamp.Id == stamp.Id) {
				GameObject.Destroy (RuleColorItem.gameObject);
				break;
			}
		}
	}

	public void OnColorClicked(StampDef stamp) {
		this.ClickHandler(this, stamp);
	}

}
