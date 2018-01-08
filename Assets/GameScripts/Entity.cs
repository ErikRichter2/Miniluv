using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	// Use this for initialization
	void Start () {		
		//PlayAnimation ("idle");
	}

	public void MoveToCheckpoint(GameObject Checkpoint) {
		PlayAnimation ("walk");
		iTween.MoveTo (gameObject, iTween.Hash("position", Checkpoint.transform.position,"time", 3.0f, "oncomplete", "MoveTweenComplete", "easetype", iTween.EaseType.linear));
	}

	public void MoveTweenComplete() {
		PlayAnimation ("idle");
		StartCoroutine (ProcessEntity());
	}

	IEnumerator ProcessEntity() {
		yield return new WaitForSeconds (2);
		PlayAnimation ("walk");
		GameObject obj = GameObject.Find ("EntityGenerator");
		transform.localScale = new Vector3(-Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
		iTween.MoveTo (gameObject, iTween.Hash("position", obj.transform.position,"time", 3.0f, "oncomplete", "DestroyEntity", "easetype", iTween.EaseType.linear));
	}

	void DestroyEntity() {
		GameObject.Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {		
	}

	void PlayAnimation(string AnimationName) {
		SpriteSheetAnimation[] animations = GetComponents<SpriteSheetAnimation> ();
		foreach (SpriteSheetAnimation animation in animations) {
			animation.Active = false;
			if (animation.AnimationName == AnimationName) {
				animation.StartAnimation ();
			}
		}
	}
}
