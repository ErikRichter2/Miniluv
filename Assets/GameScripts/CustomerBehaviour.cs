using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerBehaviour : MonoBehaviour {

	public enum STATES {
		STATE_MOVE_TO_INFOPOINT,
		STATE_MOVE_TO_NEXT_CHECKPOINT,
		STATE_PROCESS_INFOPOINT,
		STATE_PROCESS_CHECKPOINT,
		STATE_WAITING_IN_QUEUE,
		STATE_MOVE_TO_EXIT
	}

	STATES State;

	public Customer model;

	// current queue
	EntityQueue EntityQueue;

	// all scene checkpoints
	public Checkpoint[] SceneCheckpoints;

	public GameObject ExitPoint;
	public InfoPoint InfoPoint;
	int QueueIndex;
	int currentStamp;

	public void SetTaskId(int taskId) {
		this.model.taskId = taskId;
	}

	public void SetState(STATES State, bool immediateMove = false) {
		this.State = State;

		switch (this.State) {

		case STATES.STATE_WAITING_IN_QUEUE:
			StartCoroutine (ProcessWaitingInQueue ());
			break;

		case STATES.STATE_MOVE_TO_INFOPOINT:
			this.SetEntityQueue (this.InfoPoint.GetEntityQueue ());
			if (immediateMove) {
				gameObject.transform.position = this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity ());
				this.SetState (STATES.STATE_WAITING_IN_QUEUE);
			} else {
				GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueueWorldPoisition(GetEntity()), "MoveToFinished");
			}

			break;

		case STATES.STATE_MOVE_TO_NEXT_CHECKPOINT:
			Checkpoint NextCheckpoint = this.GetNextCheckpoint ();
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

		case STATES.STATE_PROCESS_INFOPOINT:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessInfopoint ());
			break;

		case STATES.STATE_PROCESS_CHECKPOINT:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessCheckpoint ());
			break;

		case STATES.STATE_MOVE_TO_EXIT:
			if (this.EntityQueue != null) {
				this.EntityQueue.RemoveEntity (this.GetEntity ());
			}
			Customers.Instance.RemoveCustomer (this.model.instanceId);
			transform.SetParent (this.ExitPoint.transform);
			GetComponent<Entity> ().MoveTo (this.ExitPoint.transform.position, "MoveToFinished");
			break;
		}
	}

	IEnumerator ProcessWaitingInQueue() {

		GetComponent<Entity> ().Idle ();

		while (true) {
			if (this.EntityQueue.IsFirst (GetEntity ())) {
				Checkpoint currentCheckpoint = this.EntityQueue.GetComponentInParent<Checkpoint> ();
				if (currentCheckpoint == null) {
					SetState (STATES.STATE_PROCESS_INFOPOINT);
				} else {
					SetState (STATES.STATE_PROCESS_CHECKPOINT);
				}
				break;
			} else {
				yield return new WaitForSeconds (0.5f);

				// check if current task still requires this stamp
				Checkpoint currentCheckpoint = this.EntityQueue.GetComponentInParent<Checkpoint> ();
				if (currentCheckpoint != null) {
					if (Rules.Instance.GetRule (this.model.taskId).HasStamp (currentCheckpoint.stamp.Id) == false) {
						this.SetState (STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
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
		yield return new WaitForSeconds (DefinitionsLoader.stampDefinition.GetItem(this.currentStamp).Time);
		Customers.Instance.AddStamp (this.model.instanceId, this.currentStamp);
		SetState (STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
	}

	IEnumerator ProcessInfopoint() {
		while (true) {
			if (Rules.Instance.GetRule(this.model.taskId).HasStamps()) {
				GetComponentInChildren<CustomerBubble> ().Hide ();
				GetComponent<BoxCollider2D> ().enabled = false;
				yield return new WaitForSeconds (int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.INFOPOINT_TIME).Value) / 1000.0f);
				Customers.Instance.SetInfo (this.model.instanceId, true);
				SetState (STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
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
			popup.ShowRule (Rules.Instance.GetRule(this.model.taskId));
		}
	}

	Checkpoint GetNextCheckpoint() {

		// next stamp
		int nextStampId = 0;

		// find first unsatisfied stamp
		foreach (int stamp in Rules.Instance.GetRule(this.model.taskId).stamps) {
			if (this.model.collectedStamps.IndexOf (stamp) == -1) {
				nextStampId = stamp;
				break;
			}
		}

		Checkpoint result = null;
		if (nextStampId != 0) {
			foreach (Checkpoint Checkpoint in this.SceneCheckpoints) {
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
			Checkpoint Checkpoint = this.EntityQueue.GetComponentInParent<Checkpoint> ();

			if (Checkpoint != null) {
				this.currentStamp = Checkpoint.stamp.Id;
				GetComponentInChildren<CustomerBubble> ().ShowColor (Checkpoint.stamp.Color);
			}
		}
	}

	public void MoveToFinished() {
		switch (this.State) {
		case STATES.STATE_MOVE_TO_INFOPOINT:
		case STATES.STATE_MOVE_TO_NEXT_CHECKPOINT:
		case STATES.STATE_WAITING_IN_QUEUE:
			this.SetState (STATES.STATE_WAITING_IN_QUEUE);
			break;
		case STATES.STATE_MOVE_TO_EXIT:
			DestroyObject (gameObject);
			break;
		}
	}


}
