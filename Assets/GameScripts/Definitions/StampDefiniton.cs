using System;
using System.Collections.Generic;
using UnityEngine;

public class StampDef : BaseDef {
	public Color Color;
	public int Time;
}

public class StampDefiniton : BaseDefinition<StampDef> {

	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			StampDef item = new StampDef ();
			item.Id = defId;
			Color newCol;
			if (ColorUtility.TryParseHtmlString (this.GetValue (defId, "color"), out newCol)) {
				item.Color = newCol;
			}
			item.Time = this.GetValueInt (defId, "time");
			this.Items.Add (item);
		}
	}

}

