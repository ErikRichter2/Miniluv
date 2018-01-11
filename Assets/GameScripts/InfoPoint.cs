using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPoint : MonoBehaviour {

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

}
