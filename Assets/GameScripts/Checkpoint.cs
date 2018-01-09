using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private EntityQueueSlot[] Slots;

	// Use this for initialization
	void Start () {
		this.Slots = GetComponentsInChildren<EntityQueueSlot> ();
	}

	public EntityQueueSlot GetFreeSlot() {
		foreach (EntityQueueSlot Slot in this.Slots) {
			if (Slot.GetEntity () == null) {
				return Slot;
			}
		}

		return null;
	}

	public EntityQueueSlot GetFirstSlot() {
		return this.Slots [0];
	}

	public EntityQueueSlot GetBetterFreeSlot(Entity Entity) {
		foreach (EntityQueueSlot BetterSlot in this.Slots) {
			if (BetterSlot.GetEntity() == Entity) {
				return null;
			}

			if (BetterSlot.GetEntity () == null) {
				return BetterSlot;
			}
		}

		return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
