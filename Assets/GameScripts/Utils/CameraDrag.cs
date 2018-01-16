using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    public float dragSpeed;

	Vector3 prevMousePosition;
 
    void Update() {

		if (Input.GetMouseButton (0) == false) {
			return;
		}

		transform.position += new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * this.dragSpeed, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * this.dragSpeed, 0f);
    }
}
