using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLight : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Entity") {
			GameObject obj =  other.transform.parent.gameObject;
			obj.SendMessage ("OnEnterLight");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Entity") {
			GameObject obj =  other.transform.parent.gameObject;
			obj.SendMessage ("OnExitLight");
		}
	}

}
