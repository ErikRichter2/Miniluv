using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	// Use this for initialization
	void Start () {		
		//PlayAnimation ("idle");
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
}
