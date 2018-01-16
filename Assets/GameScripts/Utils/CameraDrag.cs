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

        if (Input.GetMouseButtonDown(0)) {
			prevMousePosition = Input.mousePosition;
        }

		Vector3 currentMousePosition = Input.mousePosition;
 
		Vector3 diff = (currentMousePosition - prevMousePosition) * dragSpeed;
 
		transform.Translate(diff, Space.World);  

		prevMousePosition = currentMousePosition;
    }
}
