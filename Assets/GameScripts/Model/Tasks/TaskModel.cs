using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskModel : MonoBehaviour {

	// singleton
	static TaskModel Instance;

	List<ITask> tasks;

	void Start() {
		this.tasks = new List<ITask> ();
		Instance = this;
	}

	void Update() {
		float delta = Time.deltaTime;
		foreach(ITask task in this.tasks) {
			if (task.IsActive ()) {
				task.Update (delta);
			}
		}
	}

	static public T CreateTask<T>(float duration) where T: ITask, new() {
		ITask result = new T ();
		result.SetDuration (duration);
		Instance.tasks.Add (result);

		return (T)result;
	}

}
