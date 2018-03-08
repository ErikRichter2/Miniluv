using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDeskCounter : MonoBehaviour {

	public GameObject partSpritePrefab;

	private float taskTime;
	private float currentTime;
	private int taskParts;
	private int currentParts;
	private Color color;

	// Use this for initialization
	void Start () {
	}
	
	void Update () {
		if (currentTime > 0) {
			currentTime -= Time.deltaTime;
			if (currentTime <= 0) {
				
			} else {
				int nextParts = Mathf.RoundToInt (currentTime);
				if (nextParts != currentParts) {
					currentParts = nextParts;
					DrawParts ();
				}
			}
		}
	}

	public void StartTask(float taskTime, Color color) {
		this.color = color;
		this.taskTime = currentTime = taskTime;
		taskParts = currentParts = Mathf.RoundToInt (taskTime);
		DrawParts ();
	}

	private void DrawParts() {
		if (taskParts > 10) {
			taskParts = 10;
		}

		for (int i = 0; i < transform.childCount; ++i) {
			DestroyObject (transform.GetChild (i).gameObject);
		}

		float baseSize = 1f;
		float partSize = baseSize / taskParts;
		int temp = Mathf.FloorToInt(taskParts / 2);
		for (int i = 0; i < currentParts; ++i) {
			GameObject part = Instantiate<GameObject> (partSpritePrefab, transform);
			part.transform.localScale = new Vector3(0.95f * (part.transform.localScale.x / taskParts), part.transform.localScale.y, part.transform.localScale.z);
			float size = part.GetComponent<SpriteRenderer> ().bounds.size.x;
			part.transform.localPosition = new Vector3 (i * partSize - temp * partSize + ((taskParts%2)==0?(partSize/2f):0f), 0.0f, 0.0f);
			part.transform.GetComponent<SpriteRenderer>().color = color;
		}
	}

}
