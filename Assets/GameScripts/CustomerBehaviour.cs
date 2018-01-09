using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour {

	private GameObject ExitPoint;
	private List<Checkpoint> Checkpoints;
	private string State;
	private EntityQueueSlot CurrentSlot;

	// Use this for initialization
	void Start () {
		this.State = "init";
		this.Checkpoints = new List<Checkpoint> ();
	}

	public void AddCheckpoint(Checkpoint Checkpoint) {
		this.Checkpoints.Add (Checkpoint);
	}

	public void SetExitPoint(GameObject ExitPoint) {
		this.ExitPoint = ExitPoint;
	}

	public void SetState(string State) {
		this.State = State;

		switch (this.State) {
		case "init":
			GetComponent<Entity>().PlayAnimation ("idle");
			break;
		case "move_to_checkpoint":
			GetComponent<Entity> ().PlayAnimation ("walk");
			EntityQueueSlot Slot = this.Checkpoints [0].GetFreeSlot ();
			if (Slot != null) {
				MoveToSlot (Slot);
			} else {
				this.SetState ("move_to_exit");
			}
			break;
		case "process_checkpoint":
			GetComponent<Entity> ().PlayAnimation ("idle");
			StartCoroutine (ProcessCheckpoint ());
			break;
		case "move_to_exit":
			GetComponent<Entity> ().PlayAnimation ("walk");
			gameObject.transform.localScale = new Vector3 (-Mathf.Abs (gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
			this.MoveTo (this.ExitPoint);
			break;
		}
	}

	IEnumerator ProcessCheckpoint() {
		// first slot, processing
		if (this.CurrentSlot.IsFirstSlot()) {
			yield return new WaitForSeconds (2.0f);
			SetSlot (null);
			SetState ("move_to_exit");
		} else {
			// check for better slot
			EntityQueueSlot BetterSlot = this.Checkpoints [0].GetBetterFreeSlot (GetComponent<Entity> ());
			if (BetterSlot != null) {
				MoveToSlot (BetterSlot);
			} else {
				yield return new WaitForSeconds (0.5f);
				SetState ("process_checkpoint");
			}
		}
	}

	void SetSlot(EntityQueueSlot Slot) {
		if (this.CurrentSlot != null) {
			this.CurrentSlot.SetEntity (null);
		}
		this.CurrentSlot = Slot;
		if (this.CurrentSlot != null) {
			this.CurrentSlot.SetEntity (GetComponent<Entity> ());
		}
	}

	void MoveToSlot(EntityQueueSlot Slot) {
		SetSlot (Slot);
		this.MoveTo (this.CurrentSlot.gameObject);
	}

	void MoveTo(GameObject Target) {
		iTween.MoveTo (gameObject, iTween.Hash ("position", Target.transform.position, "time", 3.0f, "oncomplete", "MoveToFinished", "easetype", iTween.EaseType.linear));
	}

	public void MoveToFinished() {
		switch (this.State) {
		case "process_checkpoint":
			this.SetState ("process_checkpoint");
			break;
		case "move_to_checkpoint":
			this.SetState ("process_checkpoint");
			break;
		case "move_to_exit":
			DestroyObject (gameObject);
			break;
		default:
			this.SetState ("idle");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
