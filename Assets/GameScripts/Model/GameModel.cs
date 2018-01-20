using System;
using UnityEngine;
using System.Collections.Generic;

public class GameModel {

	public static GameModel Instance;

	Dictionary<Type, IModel> models;
	List<ISerializable> serializableModels;
	List<ITickable> tickableModels;

	public GameModel () {
		this.models = new Dictionary<Type, IModel> ();
		this.serializableModels = new List<ISerializable> ();
		this.tickableModels = new List<ITickable> ();

		GameModel.Instance = this;
	}

	T AddModel<T>(T model) where T: IModel {
		this.models [typeof(T)] = model;

		if (model is ISerializable) {
			this.serializableModels.Add (model as ISerializable);
		}

		if (model is ITickable) {
			this.tickableModels.Add (model as ITickable);
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
		this.AddModel (ScriptableObject.CreateInstance<Stamps> ());
		this.AddModel (new TaskModel ());
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

	public void Update(float delta) {
		if (this.tickableModels != null) {
			foreach (ITickable model in this.tickableModels) {
				model.Update (delta);
			}
		}
	}


}
