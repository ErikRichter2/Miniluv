using System;

public interface ITask {
	void Update (float delta);
	void SetDuration (float duration);
	float GetRemainingTime();
	float GetDuration();
	bool IsActive();
}

