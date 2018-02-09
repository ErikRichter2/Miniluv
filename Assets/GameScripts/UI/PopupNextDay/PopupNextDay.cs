using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNextDay : BasePopup {

	public Text TxtResult;

	override public void ShowPopup(bool stopGame = true) {
		base.ShowPopup (stopGame);

		int nextDayFinal = GameModel.GetModel<Customers>().GetNextDayId ();
		if (nextDayFinal != 0) {
			TxtResult.text = "Next day: " + nextDayFinal + " - " + DefinitionsLoader.daysDefinition.GetItem (nextDayFinal).Name;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBtnClick() {
		HidePopup ();
		GameModel.Instance.StartNextDay ();
	}
}
