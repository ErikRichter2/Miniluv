using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoDesk : MonoBehaviour, ITaskable {

	public SimpleTask task;

	private InfoDeskNext infoDeskNextSign;

	void Start() {
		if (Preloader.Loaded) {
			this.task = GameModel.GetModel<TaskModel>().CreateTask<SimpleTask> (int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.INFOPOINT_TIME).Value) / 1000);
			infoDeskNextSign = GameObjectUtils.GetComponentInChildren<InfoDeskNext> (gameObject);
			infoDeskNextSign.gameObject.SetActive (true);
		}
	}

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}
	/*
	public void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (0);
		}
	}
*/
	public ITask GetCurrentTask() {
		return this.task;
	}
		
	public void ShowProgress() {
		if (infoDeskNextSign != null) {
			infoDeskNextSign.gameObject.SetActive (false);
		}
		GameObjectUtils.GetComponentInChildren<TaskProgressBar> (gameObject).SetTask (this.task);
	}

	public void HideProgress() {
		if (infoDeskNextSign != null) {
			infoDeskNextSign.gameObject.SetActive (true);
		}
		GameObjectUtils.GetComponentInChildren<TaskProgressBar> (gameObject).StopTask ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//if (coll.gameObject.tag == "Entity")
		//coll.gameObject.SendMessage("ApplyDamage", 10);
		Debug.Log("Collision3");
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("Collision4");
	}


}
