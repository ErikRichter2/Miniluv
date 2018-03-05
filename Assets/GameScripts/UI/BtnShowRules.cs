using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnShowRules : MonoBehaviour {
	
	public void onClick() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (0);
		}
	}

}
