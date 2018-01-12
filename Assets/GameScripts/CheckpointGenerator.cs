using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointGenerator : MonoBehaviour {

	public Checkpoint checkpointPrefab;

	// Use this for initialization
	void Start () {
		this.CreateCheckpoints ();
	}

	void CreateCheckpoints() {
		int index = 0;
		foreach (StampDef stamp in DefinitionsLoader.stampDefinition.Items) {
			Checkpoint checkpoint = Instantiate<Checkpoint> (this.checkpointPrefab, transform);
			checkpoint.transform.localPosition = new Vector3 (index * 2.5f, 0.0f, 0.0f);
			checkpoint.SetStamp (stamp);
			++index;
		}
	}

}
