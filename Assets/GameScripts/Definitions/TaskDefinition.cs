﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum Operand {
	LESS,
	EQUAL,
	MORE
}

public class TaskDef : BaseDef {
	public string Name;
	public string Condition;
	public Operand Operand;
	public int Value;
	public int MinStamps;
	public int Duration;
	public int[] Customers;

	public bool IsOK(int CollectedCount) {
		if (this.Operand == Operand.LESS) {
			return (CollectedCount < Value);
		} else if (this.Operand == Operand.EQUAL) {
			return (CollectedCount == Value);
		} else {
			return (CollectedCount > Value);
		}
	}
}
	
public class TaskDefinition : BaseDefinition<TaskDef> {

	override protected void ProcessData() {
		foreach (int defId in this.GetKeys ()) {
			TaskDef item = new TaskDef ();
			item.Id = defId;
			item.Name = this.GetValue (defId, "Task_name");

			item.Condition = this.GetValue (defId, "Condition");
			if (item.Condition.IndexOf ("X<") == 0) {
				item.Operand = Operand.LESS;
			} else if (item.Condition.IndexOf ("X=") == 0) {
				item.Operand = Operand.EQUAL;
			} else {
				item.Operand = Operand.MORE;
			}

			string str = item.Condition;
			item.Value = int.Parse (str.Remove (0, 2));
			item.MinStamps = this.GetValueInt (defId, "Min_Stamps");
			item.Duration = this.GetValueInt (defId, "Duration");

			item.Customers = this.GetValueArrayInt (defId, "Customers");

			this.Items.Add (item);
		}
	}

}

