using System;
using System.Collections.Generic;
using UnityEngine;

public class StampDef {
	public int Id;
	public Color Color;
	public int Time;
}

public class StampDefiniton : BaseDefinition
{
	public List<StampDef> stamps;

	public StampDefiniton (){
		stamps = new List<StampDef> ();
	}

	public StampDef GetStamp(int defId) {
		foreach (StampDef item in this.stamps) {
			if (item.Id == defId) {
				return item;
			}
		}

		return null;
	}

	override protected void ProcessData() {
		List<int> keys = GetKeys ();
		foreach (int defId in keys) {
			StampDef item = new StampDef ();
			item.Id = defId;
			Color newCol;
			if (ColorUtility.TryParseHtmlString (this.GetValue (defId, "color"), out newCol)) {
				item.Color = newCol;
			}
			item.Time = this.GetValueInt (defId, "time");
			stamps.Add (item);
		}
	}

}

