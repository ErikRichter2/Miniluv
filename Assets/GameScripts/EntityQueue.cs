using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityQueue : MonoBehaviour {

	public LinkedList<Entity> Queue = new LinkedList<Entity> ();

	Vector3 GetPosition(int Index) {
		Vector3 result = new Vector3 ();

		result.x = -Index * 1.0f;
		result.y = -Index * 2.0f;
		result.z = 0;

		return result;
	}

	public void AddEntity(Entity Entity) {
		this.Queue.AddLast(Entity);
	}

	public void RemoveEntity(Entity Entity) {
		this.Queue.Remove (Entity);
	}

	public int GetQueueIndex(Entity Entity) {
		int Index = 0;

		foreach (Entity It in this.Queue) {			
			if (It == Entity) {
				break;
			}

			++Index;
		}

		return Index;
	}

	public Vector3 GetQueueWorldPoisition(Entity Entity) {		
		return transform.TransformPoint (this.GetPosition (this.GetQueueIndex(Entity)));
	}

	public Transform GetQueueContainer() {
		return transform;
	}

	public bool IsFirst(Entity Entity) {
		return (this.Queue.First.Value == Entity);
	}

}
