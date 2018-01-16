using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPoint : MonoBehaviour {

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

	public void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (0);
		}
	}

}
