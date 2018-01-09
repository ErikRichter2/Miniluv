using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int Id;

	CheckpointSlot[] Slots;

	// Use this for initialization
	void Start () {
		this.Slots = GetComponentsInChildren<CheckpointSlot> ();
	}

	public CheckpointSlot GetLastSlot() {
		return this.Slots [this.Slots.Length - 1];
	}

	public CheckpointSlot GetNextSlot(CheckpointSlot Slot) {
		for (int i = 0; i < this.Slots.Length; ++i) {
			if (this.Slots [i] == Slot) {
				if (i > 0) {
					return this.Slots [i - 1];
				}				
			}
		}

		return null;
	}

	public CheckpointSlot GetFirstSlot() {
		return this.Slots [0];
	}

	public Color GetColor() {
		return GetComponentInChildren<InfoColor> ().GetColor ();
	}


}
