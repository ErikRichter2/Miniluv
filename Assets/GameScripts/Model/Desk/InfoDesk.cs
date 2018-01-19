using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoDesk : MonoBehaviour, ITaskable {

	public SimpleTask task;

	void Start() {
		if (Preloader.Loaded) {
			this.task = GameModel.GetModel<TaskModel>().CreateTask<SimpleTask> (int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.INFOPOINT_TIME).Value) / 1000);
		}
	}

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

	public void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (0);
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
