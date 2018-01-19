using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StampDesk : MonoBehaviour, ITaskable {

	public StampDef stamp;

	public SimpleTask task;

	public void SetStamp(StampDef stamp) {
		this.stamp = stamp;
		this.task = GameModel.GetModel<TaskModel>().CreateTask<SimpleTask> (this.stamp.Time);
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

	public ITask GetCurrentTask() {
		return this.task;
	}

	public void ShowProgress() {
		GameObjectUtils.GetComponentInChildren<TaskProgressBar> (gameObject).SetTask (this.task);
	}

	public void HideProgress() {
		GameObjectUtils.GetComponentInChildren<TaskProgressBar> (gameObject).StopTask ();
	}


}
