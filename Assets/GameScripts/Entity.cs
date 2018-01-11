using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	static int ID_COUNTER = 0;

	public int EntityId;

	// Use this for initialization
	void Start () {				
		EntityId = ++ID_COUNTER;
	}

	void DestroyEntity() {
		GameObject.Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {		
	}

	public void PlayAnimation(string AnimationName) {
		SpriteSheetAnimation[] animations = GetComponents<SpriteSheetAnimation> ();
		foreach (SpriteSheetAnimation animation in animations) {
			animation.Active = false;
			if (animation.AnimationName == AnimationName) {
				animation.StartAnimation ();
			}
		}
	}

	public void MoveTo(Vector3 Position, string OnComplete = null) {
		GetComponent<Entity> ().PlayAnimation ("walk");

		if (Position.x < gameObject.transform.position.x) {
			gameObject.transform.localScale = new Vector3 (-Mathf.Abs (gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		} else {
			gameObject.transform.localScale = new Vector3 (Mathf.Abs (gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		}
			
		if (OnComplete != null) {
			iTween.MoveTo (gameObject, iTween.Hash ("position", Position, "time", 1.0f, "oncomplete", OnComplete, "easetype", iTween.EaseType.linear, "islocal", true));
		} else {
			iTween.MoveTo (gameObject, iTween.Hash ("position", Position, "time", 1.0f, "oncomplete", "MoveTo_finished", "easetype", iTween.EaseType.linear, "islocal", true));
		}
	}

	public void MoveTo_finished() {
		this.Idle ();
	}

	public void Idle() {
		GetComponent<Entity> ().PlayAnimation ("idle");
	}
}
