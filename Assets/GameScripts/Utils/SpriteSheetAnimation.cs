using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetAnimation : MonoBehaviour {

	SpriteRenderer MySpriteRenderer;
	Sprite[] AllSprites;
	float CurrentFrame;

	public string AnimationName;
	public string TextureName;
	public int FrameRate;
	public bool Active;

	// Use this for initialization
	void Start () {
		this.CurrentFrame = 0;
		this.Active = false;
		this.MySpriteRenderer = GetComponent<SpriteRenderer>();
	}

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

		this.MySpriteRenderer.sprite = this.AllSprites [Index];
	}
}
