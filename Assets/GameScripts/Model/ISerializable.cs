using System;

public interface ISerializable {
	void Save();
	void DeleteSave();
	void Load();
}

