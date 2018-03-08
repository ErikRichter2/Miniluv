using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    public float dragSpeed;

	Vector3 prevMousePosition;
 
    void Update() {

		if (BasePopup.IsPopupActive()) {
			return;
		}

		if (Input.GetMouseButton (0) == false) {
			return;
		}

        if (Input.GetMouseButtonDown(0)) {
			prevMousePosition = Input.mousePosition;
        }

		Vector3 currentMousePosition = Input.mousePosition;
 
		Vector3 diff = (prevMousePosition - currentMousePosition) * dragSpeed;

		if (transform.localPosition.y + diff.y > 2.2f) {
			diff.y = 0;
		}

		if (transform.localPosition.y + diff.y < -1.5f) {
			diff.y = 0;
		}

		if (transform.localPosition.x + diff.x < 0.5f) {
			diff.x = 0;
		}

		if (transform.localPosition.x + diff.x > 3f) {
			diff.x = 0;
		}

		transform.Translate(diff, Space.World);  

		prevMousePosition = currentMousePosition;
    }
}
