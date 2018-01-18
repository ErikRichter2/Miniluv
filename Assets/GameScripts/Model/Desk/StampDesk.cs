using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StampDesk : MonoBehaviour, ITaskable {

	public StampDef stamp;

	public void SetStamp(StampDef stamp) {
		this.stamp = stamp;
		transform.Find ("Color").GetComponent<SpriteRenderer> ().color = this.stamp.Color;
	}

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

	public void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (this.stamp.Id);
		}
	}

	public int GetTaskDuration() {
		return stamp.Time * 1000;
	}

}
