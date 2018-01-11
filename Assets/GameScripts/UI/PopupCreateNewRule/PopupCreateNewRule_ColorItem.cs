using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreateNewRule_ColorItem : MonoBehaviour {

	public RuleColorDef RuleColor;

	void Start() {
		//GetComponent<Button> ().onClick.AddListener (delegate() {this.OnClick();});
	}

	public void SetRuleColor(RuleColorDef RuleColor) {
		this.RuleColor = RuleColor;
		GetComponent<Image> ().color = this.RuleColor.Color;
	}

	public void OnClick() {
		GetComponentInParent<PopupCreateNewRule_ColorPanel> ().OnColorClicked (this.RuleColor);
	}

}
