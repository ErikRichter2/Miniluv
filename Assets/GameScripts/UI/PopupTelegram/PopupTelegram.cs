using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupTelegram : BasePopup {

	public Text body;
	private int day;

	public void ShowDay(int day) {
		this.day = day;
		body.text = DefinitionsLoader.daysDefinition.GetItem(day).Telegram;
	}

	public void OnClick() {
		HidePopup ();

		PopupNewspaper popup = BasePopup.GetPopup<PopupNewspaper> ();
		popup.ShowPopup (true);
		popup.ShowDay(day);
	}

}
