using System;
using UnityEngine;
using System.Collections.Generic;

public class GameModel {

	public static GameModel Instance;

	Dictionary<Type, IModel> models;
	List<ISerializable> serializableModels;

	public GameModel () {
		this.models = new Dictionary<Type, IModel> ();
		this.serializableModels = new List<ISerializable> ();
		GameModel.Instance = this;
	}

	T AddModel<T>(T model) where T: IModel {
		this.models [typeof(T)] = model;
		if (model is ISerializable) {
			this.serializableModels.Add (model as ISerializable);
		}

		return model;
	}

	static public T GetModel<T>() where T: IModel {
		return (T)Instance.models [typeof(T)];
	}

	static public bool IsInitialized() {
		return (Instance != null && Instance.models != null);
	}

	public void Init() {
		this.AddModel (ScriptableObject.CreateInstance<Rules> ());
		this.AddModel (ScriptableObject.CreateInstance<Customers> ());
	}

	public void Load() {
		foreach (ISerializable model in this.serializableModels) {
			model.Load ();
		}
	}

	public void Save() {
		foreach (ISerializable model in this.serializableModels) {
			model.Save ();
		}
	}

	public void DeleteSave() {
		foreach (ISerializable model in this.serializableModels) {
			model.DeleteSave ();
		}
	}


}
