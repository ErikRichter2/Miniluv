using System;
using System.Collections.Generic;

public class TaskDef {
	public int id;
	public string type;

	public TaskDef(int id, string type) {
		this.id = id;
		this.type = type;
	}
}


public class TaskDefinition : BaseDefinition {
	public List<TaskDef> tasks;

	public TaskDefinition () {
		tasks = new List<TaskDef> ();
		tasks.Add (new TaskDef (1, "WEDDING"));
		tasks.Add (new TaskDef (2, "DIVORCE"));
		tasks.Add (new TaskDef (3, "PASSPORT"));
		tasks.Add (new TaskDef (4, "FUNERAL"));
	}

	public TaskDef GetTask(int id) {
		foreach (TaskDef task in this.tasks) {
			if (task.id == id) {
				return task;
			}
		}

		return null;
	}
}

