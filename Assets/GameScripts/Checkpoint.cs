﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int Id;

	void Start () {
		transform.Find ("Color").GetComponent<SpriteRenderer> ().color = Rules.GetColor (Id);
	}

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

}
