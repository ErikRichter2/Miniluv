using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Checkpoint : MonoBehaviour {

	public int Id;

	void Start () {
		transform.Find ("Color").GetComponent<SpriteRenderer> ().color = Rules.GetColor (Id);
	}

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

	public void OnMouseDown() {
		if (EventSystem.current.IsPointerOverGameObject () == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowColor (this.Id);
		}
	}

}
