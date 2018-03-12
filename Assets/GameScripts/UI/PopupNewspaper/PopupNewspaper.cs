using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNewspaper : BasePopup {

	public Text title1;
	public Text body1;
	public Text title2;
	public Text body2;

	public void ShowDay(int day) {
		title1.text = DefinitionsLoader.daysDefinition.GetItem (day).NewsMainHeader;
		body1.text = DefinitionsLoader.daysDefinition.GetItem (day).NewsMainText;
		title2.text = DefinitionsLoader.daysDefinition.GetItem (day).NewsSecondHeader;
		body2.text = DefinitionsLoader.daysDefinition.GetItem (day).NewsThirdText;
	}

	public void OnClick() {
		HidePopup ();
		GameModel.Instance.StartNextDay ();
	}

}
