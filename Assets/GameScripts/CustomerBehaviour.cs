using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour {

	public enum STATES {
		STATE_INIT,
		STATE_MOVE_TO_NEXT_CHECKPOINT,
		STATE_PROCESS_CHECKPOINT,
		STATE_MOVE_TO_EXIT
	}

	STATES State;

	// required checkpoints
	public int[] Checkpoints;

	// current checkpoint
	Checkpoint CurrentCheckpoint;

	// all checkpoints
	List<Checkpoint> SceneCheckpoints;

	// current checkpoints queue slot
	CheckpointSlot CurrentSlot;

	GameObject ExitPoint;

	// Use this for initialization
	void Start () {
		this.SceneCheckpoints = new List<Checkpoint> ();
		this.SetState(STATES.STATE_INIT);
	}

	public void AddCheckpoint(Checkpoint Checkpoint) {
		this.SceneCheckpoints.Add (Checkpoint);
	}

	public void SetExitPoint(GameObject ExitPoint) {
		this.ExitPoint = ExitPoint;
	}

	public void SetState(STATES State) {
		this.State = State;

		switch (this.State) {

		case STATES.STATE_INIT:
			GetComponent<Entity>().PlayAnimation ("idle");
			break;

		case STATES.STATE_MOVE_TO_NEXT_CHECKPOINT:
			this.SetCurrentCheckpoint (this.GetNextCheckpoint ());
			if (this.GetCurrentCheckpoint() != null) {
				CheckpointSlot NextSlot = this.GetCurrentCheckpoint ().GetLastSlot ();
				if (NextSlot.IsAvailable()) {
					this.SetSlot (NextSlot);
					this.MoveTo (NextSlot.gameObject);
				} else {
					this.SetState (STATES.STATE_MOVE_TO_EXIT);
				}
			} else {
				this.SetState (STATES.STATE_MOVE_TO_EXIT);
			}
			break;

		case STATES.STATE_PROCESS_CHECKPOINT:
			GetComponent<Entity> ().PlayAnimation ("idle");
			StartCoroutine (ProcessCheckpoint ());
			break;

		case STATES.STATE_MOVE_TO_EXIT:
			gameObject.transform.localScale = new Vector3 (-Mathf.Abs (gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
			this.MoveTo (this.ExitPoint);
			break;
		}
	}

	IEnumerator ProcessCheckpoint() {
		// first slot, processing
		if (this.GetSlot().IsFirstSlot()) {
			yield return new WaitForSeconds (2.0f);
			SetState (STATES.STATE_MOVE_TO_NEXT_CHECKPOINT);
		} else {
			yield return new WaitForSeconds (0.5f);
			CheckpointSlot NextSlot = this.GetCurrentCheckpoint() .GetNextSlot (this.GetSlot());
			if (NextSlot.IsAvailable ()) {
				this.SetSlot (NextSlot);
				this.MoveTo (this.GetSlot().gameObject);
			} else {
				this.SetState (STATES.STATE_PROCESS_CHECKPOINT);
			}
		}
	}

	Checkpoint GetNextCheckpoint() {
		Checkpoint result = null;
		int NextCheckpointId = 0;

		if (this.GetCurrentCheckpoint() == null) {
			NextCheckpointId = this.Checkpoints [0];
		} else {
			for (int i = 0; i < this.Checkpoints.Length; ++i) {
				if (this.Checkpoints [i] == this.GetCurrentCheckpoint().Id) {
					if (i < this.Checkpoints.Length - 1) {
						NextCheckpointId = this.Checkpoints [i + 1];
						break;
					}
				}
			}
		}

		if (NextCheckpointId != 0) {
			foreach (Checkpoint Checkpoint in this.SceneCheckpoints) {
				if (Checkpoint.Id == NextCheckpointId) {
					result = Checkpoint;
					break;
				}
			}
		}

		return result;
	}

	Checkpoint GetCurrentCheckpoint() {
		return this.CurrentCheckpoint;
	}

	void SetCurrentCheckpoint(Checkpoint Checkpoint) {
		this.CurrentCheckpoint = Checkpoint;
		if (this.CurrentCheckpoint != null) {
			GetComponentInChildren<InfoColor> ().SetColor (this.CurrentCheckpoint.GetColor ());
		} else {
			GetComponentInChildren<InfoColor> ().SetActive (false);
		}
	}

	void SetSlot(CheckpointSlot Slot) {
		if (this.CurrentSlot != null) {
			this.CurrentSlot.SetEntity (null);
		}
		this.CurrentSlot = Slot;
		if (this.CurrentSlot != null) {
			this.CurrentSlot.SetEntity (GetComponent<Entity> ());
		}
	}

	CheckpointSlot GetSlot() {
		return this.CurrentSlot;
	}

	void MoveTo(GameObject Target) {
		GetComponent<Entity> ().PlayAnimation ("walk");
		iTween.MoveTo (gameObject, iTween.Hash ("position", Target.transform.position, "time", 1.0f, "oncomplete", "MoveToFinished", "easetype", iTween.EaseType.linear));
	}

	public void MoveToFinished() {
		switch (this.State) {
		case STATES.STATE_PROCESS_CHECKPOINT:
		case STATES.STATE_MOVE_TO_NEXT_CHECKPOINT:
			this.SetState (STATES.STATE_PROCESS_CHECKPOINT);
			break;
		case STATES.STATE_MOVE_TO_EXIT:
			DestroyObject (gameObject);
			break;
		default:
			this.SetState (STATES.STATE_INIT);
			break;
		}
	}


}
