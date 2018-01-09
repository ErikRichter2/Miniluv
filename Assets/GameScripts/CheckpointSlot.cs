using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSlot : MonoBehaviour {

	Entity Entity;

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetEntity(Entity Entity) {
		this.Entity = Entity;
	}

	public Entity GetEntity() {
		return this.Entity;
	}

	public bool IsFirstSlot() {
		Checkpoint Checkpoint = GetComponentInParent<Checkpoint> ();
		return (Checkpoint.GetFirstSlot () == this);
	}

	public bool IsAvailable() {
		return (GetEntity () == null);
	}
}
