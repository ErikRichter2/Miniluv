using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockArm : MonoBehaviour {

	public int Speed;

	void Update () {
		transform.Rotate (Vector3.back * 0.5f * this.Speed);
	}
}
