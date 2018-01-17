using System;
using UnityEngine;

public class GameModel {

	public static GameModel Instance;

	public Customers Customers;
	public Rules Rules;

	public GameModel () {
		GameModel.Instance = this;
	}

	public void Init() {
		this.Rules = ScriptableObject.CreateInstance<Rules>();
		this.Customers = ScriptableObject.CreateInstance<Customers>();
	}

	public void Load() {
		this.Rules.Load ();
		this.Customers.Load ();
	}

	public void Save() {
		this.Rules.Save ();
		this.Customers.Save ();
	}

	public void DeleteSave() {
		this.Rules.DeleteSave ();
		this.Customers.DeleteSave ();
	}


}
