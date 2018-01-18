using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerBehaviour : MonoBehaviour {

	public enum STATES {
		STATE_MOVE_TO_INFODESK,
		STATE_MOVE_TO_STAMPDESK,
		STATE_PROCESS_INFODESK,
		STATE_PROCESS_STAMPDESK,
		STATE_WAITING_IN_QUEUE,
		STATE_MOVE_TO_EXIT
	}

	STATES State;

	public Customer model;

	// current queue
	EntityQueue EntityQueue;

	// all scene checkpoints
	public StampDesk[] SceneStampDesks;

	public GameObject ExitPoint;
	public InfoDesk InfoDesk;
	StampDesk currentStampDesk;
	int QueueIndex;

	public void SetTaskId(int taskId) {
		this.model.taskId = taskId;
	}

	public void SetState(STATES State, bool immediateMove = false) {
		this.State = State;

		switch (this.State) {

		case STATES.STATE_WAITING_IN_QUEUE:
			StartCoroutine (ProcessWaitingInQueue ());
			break;

		case STATES.STATE_MOVE_TO_INFODESK:
			this.SetEntityQueue (this.InfoDesk.GetEntityQueue ());
			if (immediateMove) {
				gameObject.transform.position = this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity ());
				this.SetState (STATES.STATE_WAITING_IN_QUEUE);
			} else {
				GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueueWorldPoisition(GetEntity()), "MoveToFinished");
			}

			break;

		case STATES.STATE_MOVE_TO_STAMPDESK:
			StampDesk NextCheckpoint = this.GetNextCheckpoint ();
			if (NextCheckpoint != null) {
				this.SetEntityQueue (NextCheckpoint.GetEntityQueue());
				if (immediateMove) {
					gameObject.transform.position = this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity ());
					this.SetState (STATES.STATE_WAITING_IN_QUEUE);
				} else {
					GetComponent<Entity> ().MoveTo (this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity()), "MoveToFinished");
				}
			} else {
				this.SetEntityQueue (null);
				this.SetState (STATES.STATE_MOVE_TO_EXIT);
			}
			break;

		case STATES.STATE_PROCESS_INFODESK:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessInfopoint ());
			break;

		case STATES.STATE_PROCESS_STAMPDESK:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessCheckpoint ());
			break;

		case STATES.STATE_MOVE_TO_EXIT:
			if (this.EntityQueue != null) {
				this.EntityQueue.RemoveEntity (this.GetEntity ());
			}
			GameModel.GetModel<Customers>().RemoveCustomer (this.model.instanceId);
			transform.SetParent (this.ExitPoint.transform);
			GetComponent<Entity> ().MoveTo (this.ExitPoint.transform.position, "MoveToFinished");
			break;
		}
	}

	IEnumerator ProcessWaitingInQueue() {

		GetComponent<Entity> ().Idle ();

		while (true) {
			if (this.EntityQueue.IsFirst (GetEntity ())) {
				StampDesk currentCheckpoint = this.EntityQueue.GetComponentInParent<StampDesk> ();
				if (currentCheckpoint == null) {
					SetState (STATES.STATE_PROCESS_INFODESK);
				} else {
					SetState (STATES.STATE_PROCESS_STAMPDESK);
				}
				break;
			} else {
				yield return new WaitForSeconds (0.5f);

				// check if current task still requires this stamp
				StampDesk currentCheckpoint = this.EntityQueue.GetComponentInParent<StampDesk> ();
				if (currentCheckpoint != null) {
					if (GameModel.GetModel<Rules>().GetRule (this.model.taskId).HasStamp (currentCheckpoint.stamp.Id) == false) {
						this.SetState (STATES.STATE_MOVE_TO_STAMPDESK);
						break;
					}
				}


				int NewQueueIndex = this.EntityQueue.GetQueueIndex (GetEntity());
				if (this.QueueIndex != NewQueueIndex) {
					this.QueueIndex = NewQueueIndex;
					GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueueWorldPoisition(GetEntity()), "MoveToFinished");
					break;
				}
			}
		}
	}

	Entity GetEntity() {
		return GetComponent<Entity> ();
	}
		
	IEnumerator ProcessCheckpoint() {
		yield return new WaitForSeconds (this.currentStampDesk.GetTaskDuration() / 1000);
		GameModel.GetModel<Customers>().AddStamp (this.model.instanceId, this.currentStampDesk.stamp.Id);
		SetState (STATES.STATE_MOVE_TO_STAMPDESK);
	}

	IEnumerator ProcessInfopoint() {
		while (true) {
			if (GameModel.GetModel<Rules>().GetRule(this.model.taskId).HasStamps()) {
				GetComponentInChildren<CustomerBubble> ().Hide ();
				GetComponent<BoxCollider2D> ().enabled = false;
				yield return new WaitForSeconds (this.InfoDesk.GetTaskDuration() / 1000);
				GameModel.GetModel<Customers>().SetInfo (this.model.instanceId, true);
				SetState (STATES.STATE_MOVE_TO_STAMPDESK);
				break;
			} else {
				GetComponentInChildren<CustomerBubble> ().ShowInfoBubble ();
				GetComponent<BoxCollider2D> ().enabled = true;
				yield return new WaitForSeconds (0.5f);
			}
		}
	}

	void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupCreateNewRule popup = BasePopup.GetPopup<PopupCreateNewRule>();
			popup.ShowRule (GameModel.GetModel<Rules>().GetRule(this.model.taskId));
		}
	}

	StampDesk GetNextCheckpoint() {

		// next stamp
		int nextStampId = 0;

		// find first unsatisfied stamp
		foreach (int stamp in GameModel.GetModel<Rules>().GetRule(this.model.taskId).stamps) {
			if (this.model.collectedStamps.IndexOf (stamp) == -1) {
				nextStampId = stamp;
				break;
			}
		}

		StampDesk result = null;
		if (nextStampId != 0) {
			foreach (StampDesk Checkpoint in this.SceneStampDesks) {
				if (Checkpoint.stamp.Id == nextStampId) {
					result = Checkpoint;
					break;
				}
			}
		}

		return result;
	}

	EntityQueue GetEntityQueue() {
		return this.EntityQueue;
	}

	void SetEntityQueue(EntityQueue EntityQueue) {
		if (this.EntityQueue != null) {
			this.EntityQueue.RemoveEntity (GetEntity());
		}

		if (EntityQueue == null) {
			this.EntityQueue = null;
			GetComponentInChildren<CustomerBubble> ().Hide ();
		} else {
			this.EntityQueue = EntityQueue;
			this.EntityQueue.AddEntity (GetEntity());
			this.QueueIndex = this.EntityQueue.GetQueueIndex (GetEntity());
			GetEntity ().SetSortOrder (this.QueueIndex);
			transform.SetParent(this.EntityQueue.GetQueueContainer ());
			StampDesk stampDesk = this.EntityQueue.GetComponentInParent<StampDesk> ();

			if (stampDesk != null) {
				this.currentStampDesk = stampDesk;
				GetComponentInChildren<CustomerBubble> ().ShowColor (stampDesk.stamp.Color);
			}
		}
	}

	public void MoveToFinished() {
		switch (this.State) {
		case STATES.STATE_MOVE_TO_INFODESK:
		case STATES.STATE_MOVE_TO_STAMPDESK:
		case STATES.STATE_WAITING_IN_QUEUE:
			this.SetState (STATES.STATE_WAITING_IN_QUEUE);
			break;
		case STATES.STATE_MOVE_TO_EXIT:
			DestroyObject (gameObject);
			break;
		}
	}


}
