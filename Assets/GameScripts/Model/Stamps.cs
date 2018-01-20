using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

[System.Serializable]
public class Stamp {
	public int id;
	public int collectedCount;

	public void AddCollectedCount() {
		++this.collectedCount;
		GameModel.GetModel<Stamps> ().Save ();
	}
}

[System.Serializable]
public class Stamps : ScriptableObject, IModel, ISerializable {

	[SerializeField]
	List<Stamp> stamps;

	public Stamps() {
		this.stamps = new List<Stamp> ();

		// inital stamps
		foreach (StampDef stampDef in DefinitionsLoader.stampDefinition.Items) {
			Stamp stamp = new Stamp ();
			stamp.id = stampDef.Id;
			stamp.collectedCount = 0;
			this.stamps.Add (stamp);
		}
	}

	public Stamp GetStamp(int stampId) {
		foreach (Stamp It in this.stamps) {
			if (It.id == stampId) {
				return It;
			}
		}

		Assert.IsTrue (false);
		return null;
	}

	public void Load() {
		string savePath = Path.Combine (Application.persistentDataPath, "stamps.txt");
		if (File.Exists (savePath)) {
			string saveData = File.ReadAllText(savePath);
			JsonUtility.FromJsonOverwrite(saveData, this);
		}
	}

	public void Save() {
		string savePath = Path.Combine (Application.persistentDataPath, "stamps.txt");
		string saveData = JsonUtility.ToJson (this);
		File.WriteAllText (savePath, saveData);
	}

	public void DeleteSave() {
		File.Delete(Path.Combine (Application.persistentDataPath, "stamps.txt"));
	}

}
