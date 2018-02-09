using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

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
	public CustomerGenerator CustomerGenerator;
	StampDesk currentStampDesk;
	int QueueIndex;

	bool disposed;

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

			if (this.model.HasAllStampsCollected ()) {
				GameModel.GetModel<Rules> ().GetRule (this.model.taskId).AddCollectedCount ();
				this.SetEntityQueue (null);
				this.SetState (STATES.STATE_MOVE_TO_EXIT);
			} else {
				StampDesk nextStampDesk = null;
				int firstUncollectedStamp = this.model.GetFirstUncollectedStamp ();
				if (firstUncollectedStamp != 0) {
					foreach (StampDesk stampDesk in this.SceneStampDesks) {
						if (stampDesk.stamp.Id == firstUncollectedStamp) {
							nextStampDesk = stampDesk;
							break;
						}
					}
				}

				Assert.IsNotNull (nextStampDesk);
				if (nextStampDesk != null) {
					this.SetEntityQueue (nextStampDesk.GetEntityQueue ());
					if (immediateMove) {
						gameObject.transform.position = this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity ());
						this.SetState (STATES.STATE_WAITING_IN_QUEUE);
					} else {
						GetComponent<Entity> ().MoveTo (this.GetEntityQueue ().GetQueueWorldPoisition (GetEntity ()), "MoveToFinished");
					}
				}
			}
			break;

		case STATES.STATE_PROCESS_INFODESK:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessInfoDesk ());
			break;

		case STATES.STATE_PROCESS_STAMPDESK:
			GetComponent<Entity> ().Idle ();
			StartCoroutine (ProcessStampDesk ());
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

				if (disposed) {
					yield break;
				}

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
					this.SetQueueIndex (NewQueueIndex);
					GetComponent<Entity> ().MoveTo (this.GetEntityQueue().GetQueueWorldPoisition(GetEntity()), "MoveToFinished");
					break;
				}
			}
		}
	}

	void SetQueueIndex(int index) {
		this.QueueIndex = index;
		GetEntity ().SetSortOrder (this.QueueIndex);
	}

	Entity GetEntity() {
		return GetComponent<Entity> ();
	}
		
	IEnumerator ProcessStampDesk() {
		this.currentStampDesk.ShowProgress ();
		yield return new WaitForSeconds (this.currentStampDesk.task.StartTask(this.currentStampDesk.task.GetDuration ()));

		if (disposed) {
			yield break;
		}

		this.currentStampDesk.model.AddCollectedCount ();
		this.currentStampDesk.HideProgress ();
		GameModel.GetModel<Customers>().AddStamp (this.model.instanceId, this.currentStampDesk.stamp.Id);
		SetState (STATES.STATE_MOVE_TO_STAMPDESK);
	}

	IEnumerator ProcessInfoDesk() {
		while (true) {
			if (GameModel.GetModel<Rules>().GetRule(this.model.taskId).HasStamps()) {
				//GetComponentInChildren<CustomerBubble> ().Hide ();
				GetComponent<BoxCollider2D> ().enabled = false;
				this.InfoDesk.ShowProgress ();
				yield return new WaitForSeconds (this.InfoDesk.task.StartTask(this.InfoDesk.task.GetDuration ()));

				if (disposed) {
					yield break;
				}

				this.InfoDesk.HideProgress ();
				GameModel.GetModel<Customers>().SetInfo (this.model.instanceId, true);
				SetState (STATES.STATE_MOVE_TO_STAMPDESK);
				break;
			} else {
				//GetComponentInChildren<CustomerBubble> ().ShowInfoBubble ();
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

	EntityQueue GetEntityQueue() {
		return this.EntityQueue;
	}

	void SetEntityQueue(EntityQueue EntityQueue) {
		if (this.EntityQueue != null) {
			this.EntityQueue.RemoveEntity (GetEntity());
		}

		if (EntityQueue == null) {
			this.EntityQueue = null;
			//GetComponentInChildren<CustomerBubble> ().Hide ();
		} else {
			this.EntityQueue = EntityQueue;
			this.EntityQueue.AddEntity (GetEntity());
			this.SetQueueIndex(this.EntityQueue.GetQueueIndex (GetEntity()));
			GetEntity ().SetSortOrder (this.QueueIndex);
			transform.SetParent(this.EntityQueue.GetQueueContainer ());
			StampDesk stampDesk = this.EntityQueue.GetComponentInParent<StampDesk> ();

			if (stampDesk != null) {
				this.currentStampDesk = stampDesk;
				//GetComponentInChildren<CustomerBubble> ().ShowColor (stampDesk.stamp.Color);
			}

			//this.refreshShadow ();
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
			this.CustomerGenerator.DestroyCustomer (this);
			break;
		}
	}

	public void DestroyCustomer() {
		model = null;
		if (EntityQueue != null) {
			EntityQueue.RemoveEntity (this.GetEntity());
			EntityQueue = null;
		}

		SceneStampDesks = null;
		ExitPoint = null;
		InfoDesk = null;
		CustomerGenerator = null;
		currentStampDesk = null;
		QueueIndex = 0;
		disposed = true;
	}
	/*
	void refreshShadow() {
		if (this.currentStampDesk != null) {
			int index = this.EntityQueue.GetQueueIndex (this.GetEntity ());
			GetComponent<SpriteRenderer> ().color = index == 0 ? Color.white : Color.black;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.black;
		}
	}
*/

}
