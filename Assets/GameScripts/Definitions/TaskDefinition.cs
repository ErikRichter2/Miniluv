using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskDef : BaseDef {
	public string Name;
}
	
public class TaskDefinition : BaseDefinition<TaskDef> {

	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			TaskDef item = new TaskDef ();
			item.Id = defId;
			item.Name = this.GetValue (defId, "name");
			this.Items.Add (item);
		}
	}

}

