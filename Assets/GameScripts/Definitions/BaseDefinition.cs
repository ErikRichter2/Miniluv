﻿using System;
using System.Collections.Generic;

public class BaseDef {
	public int Id;
}

public class BaseDefinition<T> where T : BaseDef
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

	protected int GetValueInt(int defId, string name) {
		return int.Parse(this.data [defId] [this.GetIndex(name)]);
	}

	protected List<int> GetKeys() {
		List<int> res = new List<int>();
		foreach (KeyValuePair<int, string[]> It in this.data) {
			res.Add(It.Key);
		}

		return res;
	}
}

