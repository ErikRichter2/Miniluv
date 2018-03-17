using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDeskAnimation : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Sprite idle;
	public List<Sprite> anim;
	public int animSpeed;

	private int frameCounter;
	private int animIndex;
	private bool isAnim;

	// Use this for initialization
	void Start () {
		animIndex = 0;
	}

	public void ShowAnim() {
		isAnim = true;
		animIndex = 0;
		frameCounter = 0;
		SetAnimSprite ();
	}

	public void ShowIdle() {
		isAnim = false;
		SetIdleSprite ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnim) {
			frameCounter++;
			if (frameCounter >= animSpeed) {
				frameCounter = 0;
				animIndex++;

				if (animIndex > anim.Count - 1) {
					animIndex = 0;
				}

				SetAnimSprite ();
			}
		}
	}

	void SetAnimSprite() {
		spriteRenderer.sprite = anim [animIndex];
	}

	void SetIdleSprite() {
		spriteRenderer.sprite = idle;
	}


}
