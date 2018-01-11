using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBubble : MonoBehaviour {

	public SpriteRenderer MissingRuleBubble;
	public SpriteRenderer ColorBubble;

	public void ShowColor(Color Color) {
		MissingRuleBubble.enabled = false;
		ColorBubble.enabled = true;
		ColorBubble.color = Color;
	}

	public void ShowInfoBubble() {
		MissingRuleBubble.enabled = true;
		ColorBubble.enabled = false;
	}

	public Color GetColor() {
		return ColorBubble.color;
	}

	public void Hide() {
		MissingRuleBubble.enabled = false;
		ColorBubble.enabled = false;
	}


}
