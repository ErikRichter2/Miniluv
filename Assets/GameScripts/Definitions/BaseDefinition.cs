using System;
using System.Collections.Generic;

public class BaseDefinition
{
	string[] header;
	Dictionary<int, string[]> data;

	public BaseDefinition ()
	{
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

		this.ProcessData ();
	}

	virtual protected void ProcessData() {
	}

	public int GetIndex(string name) {
		for (int i = 0; i < this.header.Length; ++i) {
			if (this.header [i] == name) {
				return i;
			}
		}

		return 0;
	}

	public string GetValue(int defId, string name) {
		return this.data [defId] [this.GetIndex(name)];
	}

	public int GetValueInt(int defId, string name) {
		return int.Parse(this.data [defId] [this.GetIndex(name)]);
	}

	public List<int> GetKeys() {
		List<int> res = new List<int>();
		foreach (KeyValuePair<int, string[]> It in this.data) {
			res.Add(It.Key);
		}

		return res;
	}
}

