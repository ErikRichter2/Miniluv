using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	static int ID_COUNTER = 0;

	public int instanceId;
	public int defId;

	public void SetEntityDef(CustomerDef defItem, int instanceId) {
		this.defId = defItem.Id;
		this.instanceId = instanceId != 0 ? instanceId : ++ID_COUNTER;

		SpriteSheetAnimation animation;

		animation = gameObject.AddComponent<SpriteSheetAnimation> ();
		animation.AnimationName = "idle";
		animation.TextureName = defItem.Asset + "-" + animation.AnimationName;
		animation.FrameRate = 10;

		animation = gameObject.AddComponent<SpriteSheetAnimation> ();
		animation.AnimationName = "walk";
		animation.TextureName = defItem.Asset + "-" + animation.AnimationName;
		animation.FrameRate = 10;
	}

	void DestroyEntity() {
		GameObject.Destroy (gameObject);
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

		Vector3 newScale = new Vector3 ( Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		if (Position.x < gameObject.transform.localPosition.x) {
			newScale.x = -newScale.x;
		}

		gameObject.transform.localScale = newScale;
			
		float speed = DefinitionsLoader.customerDefinition.GetItem (this.defId).Speed;
		float distance = Vector3.Distance (gameObject.transform.localPosition, Position);
		float time = distance / (speed * 0.25f);

		string onComplete = OnComplete != null ? OnComplete : "MoveTo_finished";
		iTween.MoveTo (gameObject, iTween.Hash ("position", Position, "time", time, "oncomplete", onComplete, "easetype", iTween.EaseType.linear, "islocal", true));
	}

	public void MoveTo_finished() {
		this.Idle ();
	}

	public void Idle() {
		GetComponent<Entity> ().PlayAnimation ("idle");
	}

	public void SetSortOrder(int order) {
		GetComponent<SpriteRenderer> ().sortingOrder = 100 + order;
		GetComponentInChildren<CustomerBubble> ().RefreshSortOrder ();
	}
}
