using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskModel : IModel, ITickable {

	List<ITask> tasks;

	public TaskModel() {
		this.tasks = new List<ITask> ();
	}

	public void Clear() {
		if (this.tasks != null) {
			this.tasks.Clear ();
		}
	}

	public void UpdateModel(float delta) {
		foreach(ITask task in this.tasks) {
			if (task.IsActive ()) {
				task.Update (delta);
			}
		}
	}

	public T CreateTask<T>(float duration) where T: ITask, new() {
		ITask result = new T ();
		result.SetDuration (duration);
		this.tasks.Add (result);

		return (T)result;
	}

}
