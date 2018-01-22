using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampAlphaEffect : MonoBehaviour {

	int cnt;

	// Use this for initialization
	void Start () {
		this.FadeEffect ();
	}
	
	// Update is called once per frame
	void Update () {		
	}

	void FadeEffect() {
		this.cnt = Random.Range(3, 7);
		this.Fade (Random.Range (0.05f, 0.3f), Random.Range(0f, 20f));
	}

	void Fade(float time, float delay) {
		Hashtable tweenParams = new Hashtable();
		iTween.Stop (gameObject);

		float nextAlpha = Random.Range (0f, 1f);
		if (this.cnt == 1) {
			nextAlpha = 1f;			
		}

		tweenParams.Add("alpha", nextAlpha);
		tweenParams.Add("time", time);
		tweenParams.Add("delay", delay);
		tweenParams.Add("oncomplete", "OnFadeCompleted");

		iTween.FadeTo(gameObject, tweenParams);
	}

	public void OnFadeCompleted() {
		--this.cnt;
		if (this.cnt > 0) {
			this.Fade (Random.Range (0.01f, 0.5f), 0f);
		} else {
			this.FadeEffect ();
		}
	}

}
