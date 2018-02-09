using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDef : BaseDef {
	public string Value;
	public string Desc;
}


public class ConfigDefinition : BaseDefinition<ConfigDef> {

	public const int CUSTOMERS_PER_MINUTE = 1;
	public const int INFOPOINT_TIME = 2;
	public const int DAY_LENGTH = 3;

	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			ConfigDef item = new ConfigDef ();
			item.Id = defId;
			item.Value = this.GetValue (defId, "value");
			item.Desc = this.GetValue (defId, "desc");
			this.Items.Add (item);
		}
	}

}

