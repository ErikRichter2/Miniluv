using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StampDesk : MonoBehaviour, ITaskable {

	public StampDef stamp;
	public SimpleTask task;
	public Stamp model;
	public SpriteRenderer spriteRenderer;

	private StampDeskCounter stampDeskCounter;
	private StampDeskAnimation stampDeskAnimation;

	public void SetStamp(StampDef stamp, int index) {
		this.stamp = stamp;
		this.task = GameModel.GetModel<TaskModel>().CreateTask<SimpleTask> (this.stamp.Time);
		this.model = GameModel.GetModel<Stamps> ().GetStamp (this.stamp.Id);
		transform.Find ("Color").GetComponent<SpriteRenderer> ().color = this.stamp.Color;

		GameObjectUtils.GetComponentInChildren<StampDeskCollectedStampsCounter> (gameObject).SetStampDesk (this.model);
		stampDeskCounter = GameObjectUtils.GetComponentInChildren<StampDeskCounter> (gameObject);
		stampDeskAnimation = GetComponent<StampDeskAnimation> ();

		spriteRenderer.sortingOrder = index;
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
		stampDeskCounter.StartTask (this.task.GetDuration(), this.stamp.Color);
		stampDeskAnimation.ShowAnim ();
	}

	public void HideProgress() {
		GameObjectUtils.GetComponentInChildren<TaskProgressBar> (gameObject).StopTask ();
		stampDeskAnimation.ShowIdle ();
	}



}
