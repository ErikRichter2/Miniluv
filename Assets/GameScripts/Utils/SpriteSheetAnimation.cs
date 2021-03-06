﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetAnimation : MonoBehaviour {

	Sprite[] AllSprites;
	float CurrentFrame;

	public string AnimationName;

	[HideInInspector]
	public string TextureName;

	public int FrameRate;
	public bool Active;

	public void StartAnimation() {
		this.CurrentFrame = 0;
		this.Active = true;
		this.AllSprites = Resources.LoadAll<Sprite> ("Entities/" + TextureName);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.Active == false) {
			return;
		}

		this.CurrentFrame += this.FrameRate / (1.0f / Time.smoothDeltaTime);
		int Index = (int)this.CurrentFrame;
		if (Index > this.AllSprites.Length - 1) {
			Index = 0;
			this.CurrentFrame = 0;
		}

		GetComponent<SpriteRenderer>().sprite = this.AllSprites [Index];
	}
}
