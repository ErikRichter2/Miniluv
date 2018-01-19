using System.Collections;
using System.Collections.Generic;

public class SimpleTask : ITask {

	float duration;
	float durationLeft;
	bool isActive;

	public void Update (float delta) {
		this.durationLeft -= delta;

		if (this.durationLeft <= 0) {
			isActive = false;
			this.durationLeft = 0;
		}
	}

	public void SetDuration (float duration) {
		this.duration = duration;
	}

	public float GetRemainingTime() {
		return this.durationLeft;
	}

	public float GetDuration() {
		return this.duration;
	}

	public float StartTask(float duration) {
		this.duration = duration;
		this.durationLeft = duration;
		isActive = true;

		return this.duration;
	}

	public bool IsActive() {
		return this.isActive;
	}

}
