using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDesksGenerator : MonoBehaviour {

	public StampDesk stampDeskPrefab;

	void Start () {
		if (Preloader.Loaded) {
			int index = 0;
			foreach (StampDef stamp in DefinitionsLoader.stampDefinition.Items) {
				StampDesk stampDesk = Instantiate<StampDesk> (this.stampDeskPrefab, transform);
				stampDesk.transform.localPosition = new Vector3 (index * 2.5f, 0.0f, 0.0f);
				stampDesk.SetStamp (stamp);
				++index;
			}

			GameObject.Find ("EntityGenerator").GetComponent<CustomerGenerator> ().Init ();
		}
	}

}
