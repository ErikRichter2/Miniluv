using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDef : BaseDef {
	public int Speed;
	public string Asset;
}


public class CustomerDefinition : BaseDefinition<CustomerDef> {

	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			CustomerDef item = new CustomerDef ();
			item.Id = defId;
			item.Speed = this.GetValueInt (defId, "speed");
			item.Asset = this.GetValue (defId, "asset");
			this.Items.Add (item);
		}
	}

}

