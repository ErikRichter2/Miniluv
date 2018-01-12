using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreateNewRule_ColorItem : MonoBehaviour {

	public StampDef stamp;

	void Start() {
		//GetComponent<Button> ().onClick.AddListener (delegate() {this.OnClick();});
	}

	public void SetStampDef(StampDef stamp) {
		this.stamp = stamp;
		GetComponent<Image> ().color = this.stamp.Color;
	}

	public void OnClick() {
		GetComponentInParent<PopupCreateNewRule_ColorPanel> ().OnColorClicked (this.stamp);
	}

}
