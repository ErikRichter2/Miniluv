using System;
using UnityEngine;
using System.Collections.Generic;

public class GameModel {

	public static GameModel Instance;

	Dictionary<Type, IModel> models;
	Dictionary<Type, ISerializable> serializableModels;

	public GameModel () {
		this.models = new Dictionary<Type, IModel> ();
		this.serializableModels = new Dictionary<Type, ISerializable> ();
		GameModel.Instance = this;
	}

	T AddModel<T>(T model) where T: IModel {
		this.models [typeof(T)] = model;
		return model;
	}

	static public T GetModel<T>() where T: IModel {
		return (T)Instance.models [typeof(T)];
	}

	public void Init() {
		this.AddModel (ScriptableObject.CreateInstance<Rules> ());
		this.AddModel (ScriptableObject.CreateInstance<Customers> ());
	}

	public void Load() {
		foreach (KeyValuePair<Type, ISerializable> model in this.serializableModels) {
			model.Value.Load ();
		}
	}

	public void Save() {
		foreach (KeyValuePair<Type, ISerializable> model in this.serializableModels) {
			model.Value.Save ();
		}
	}

	public void DeleteSave() {
		foreach (KeyValuePair<Type, ISerializable> model in this.serializableModels) {
			model.Value.DeleteSave ();
		}
	}


}
