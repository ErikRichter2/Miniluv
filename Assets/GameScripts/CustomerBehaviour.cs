using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerBehaviour : MonoBehaviour {

	public enum STATES {
		STATE_INIT,
		STATE_MOVE_TO_INFOPOINT,
		STATE_MOVE_TO_NEXT_CHECKPOINT,
		STATE_PROCESS_INFOPOINT,
		STATE_PROCESS_CHECKPOINT,
		STATE_WAITING_IN_QUEUE,
		STATE_MOVE_TO_EXIT
	}

	STATES State;

	// required rule
	Rule Rule;

	// done checkpoints
	List<int> CheckpointsDone;

	// current queue
	EntityQueue EntityQueue;

	// all scene checkpoints
	List<Checkpoint> SceneCheckpoints;

	GameObject ExitPoint;
	InfoPoint InfoPoint;
	int QueueIndex;
	int CurrentColor;

	// Use this for initialization
	void Start () {
		this.SceneCheckpoints = new List<Checkpoint> ();
		this.SetState(STATES.STATE_INIT);
		this.CheckpointsDone = new List<int> ();
	}

	public void AddCheckpoint(Checkpoint Checkpoint) {
		this.SceneCheckpoints.Add (Checkpoint);
	}

	public void SetExitPoint(GameObject ExitPoint) {
		this.ExitPoint = ExitPoint;
	}

	public void SetInfoPoint(InfoPoint InfoPoint) {
		this.InfoPoint = InfoPoint;
	}

	public void SetRule(Rule Rule) {
		this.Rule = Rule;
	}

	public void SetState(STATES State) {
		this.State = State;

		switch (this.State) {

		case STATES.STATE_INIT:
			GetEntity ().Idle ();
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponentInChildren<CustomerBubble> ().Hide ();
			break;

		case STATES.STATE_WAITING_IN_QUEUE:
			StartCoroutine (ProcessWaitingInQueue ());
			break;

		case STATES.STATE_MOVE_TO_INFOPOINT:
			this.SetEntityQueue (this.InfoPoint.GetEntityQueue());
			GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueuePoisition(GetEntity()), "MoveToFinished");
			break;

		case STATES.STATE_MOVE_TO_NEXT_CHECKPOINT:
			Checkpoint NextCheckpoint = this.GetNextCheckpoint ();
			if (NextCheckpoint != null) {
				this.SetEntityQueue (NextCheckpoint.GetEntityQueue());
				GetComponent<Entity> ().MoveTo (this.GetEntityQueue ().GetQueuePoisition (GetEntity()), "MoveToFinished");
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
			GetComponent<Entity> ().MoveTo (this.ExitPoint.transform.position, "MoveToFinished");
			break;
		}
	}

	IEnumerator ProcessWaitingInQueue() {

		GetComponent<Entity> ().Idle ();

		while (true) {
			if (this.EntityQueue.IsFirst (GetEntity ())) {
				Checkpoint CurrentCheckpoint = this.EntityQueue.GetComponentInParent<Checkpoint> ();
				if (CurrentCheckpoint == null) {
					SetState (STATES.STATE_PROCESS_INFOPOINT);
				} else {
					SetState (STATES.STATE_PROCESS_CHECKPOINT);
				}
				break;
			} else {
				yield return new WaitForSeconds (0.5f);
				int NewQueueIndex = this.EntityQueue.GetQueueIndex (GetEntity());
				if (this.QueueIndex != NewQueueIndex) {
					this.QueueIndex = NewQueueIndex;
					GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueuePoisition(GetEntity()), "MoveToFinished");
					break;
				}
			}
		}
	}

	Entity GetEntity() {
		return GetComponent<Entity> ();
	}
		
	IEnumerator ProcessCheckpoint() {
		yield return new WaitForSeconds (5.0f);
		this.CheckpointsDone.Add (this.CurrentColor);
		SetState (STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
	}

	IEnumerator ProcessInfopoint() {
		while (true) {
			if (this.Rule.RuleColors.Count > 0) {
				GetComponentInChildren<CustomerBubble> ().Hide ();
				GetComponent<BoxCollider2D> ().enabled = false;
				yield return new WaitForSeconds (1.0f);
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
		if (EventSystem.current.IsPointerOverGameObject () == false) {
			PopupCreateNewRule popup = BasePopup.GetPopup<PopupCreateNewRule>();
			popup.ShowRule (this.Rule);
		}
	}

	Checkpoint GetNextCheckpoint() {

		int NextCheckpointId = 0;
		foreach (int RuleColorId in this.Rule.RuleColors) {
			if (this.CheckpointsDone.IndexOf (RuleColorId) == -1) {
				NextCheckpointId = RuleColorId;
				break;
			}
		}

		Checkpoint result = null;
		if (NextCheckpointId != 0) {
			foreach (Checkpoint Checkpoint in this.SceneCheckpoints) {
				if (Checkpoint.stamp.Id == NextCheckpointId) {
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
			transform.SetParent (null);
			GetComponentInChildren<CustomerBubble> ().Hide ();
		} else {
			this.EntityQueue = EntityQueue;
			this.EntityQueue.AddEntity (GetEntity());
			this.QueueIndex = this.EntityQueue.GetQueueIndex (GetEntity());
			GetEntity ().SetSortOrder (this.QueueIndex);
			transform.SetParent(this.EntityQueue.GetQueueContainer ());

			Checkpoint Checkpoint = this.EntityQueue.GetComponentInParent<Checkpoint> ();
			if (Checkpoint != null) {
				this.CurrentColor = Checkpoint.stamp.Id;
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
