using System;
using System.Collections.Generic;

public class BaseDef {
	public int Id;
}

public class BaseDefinition<T> : IDefinition where T : BaseDef
{
	public List<T> Items;

	string[] header;
	Dictionary<int, string[]> data;

	public BaseDefinition (){
		Items = new List<T> ();
	}

	public T GetItem(int Id) {
		foreach (T It in this.Items) {
			if (It.Id == Id) {
				return It;
			}
		}

		return null;
	}

	public void Parse(string data) {
		this.data = new Dictionary<int, string[]> ();
		bool isHeader = true;
		string[] rows = data.Split (new string[]{ "\r\n", "\n" }, StringSplitOptions.None);
		foreach (string row in rows) {
			string[] items = row.Split (new string[]{ "|" }, StringSplitOptions.None);
			if (isHeader) {
				isHeader = false;
				header = items;
			} else {
				int defId = int.Parse (items [0]);
				this.data [defId] = items;
			}
		}

		this.Items.Clear ();
		this.ProcessData ();
	}

	virtual protected void ProcessData() {		
	}

	protected int GetIndex(string name) {
		for (int i = 0; i < this.header.Length; ++i) {
			if (this.header [i] == name) {
				return i;
			}
		}

		return 0;
	}

	protected string GetValue(int defId, string name) {
		return this.data [defId] [this.GetIndex(name)];
	}

	protected int[] GetValueArrayInt(int defId, string name) {
		string value = this.GetValue (defId, name);
		if (value == null || value == "") {
			return new int[0];
		} else {
			string[] strArr = value.Split(',');
			int[] intArr = new int[strArr.Length];

			for (int i = 0; i < strArr.Length; ++i) {
				intArr [i] = int.Parse (strArr [i]);
			}

			return intArr;
		}
	}

	protected int GetValueInt(int defId, string name) {
		string value = this.GetValue (defId, name);
		if (value == null || value == "") {
			return 0;
		} else {
			return int.Parse(value);
		}
	}

	protected List<int> GetKeys() {
		List<int> res = new List<int>();
		foreach (KeyValuePair<int, string[]> It in this.data) {
			res.Add(It.Key);
		}

		return res;
	}
}

