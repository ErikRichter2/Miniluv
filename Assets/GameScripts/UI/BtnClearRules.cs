﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClearRules : MonoBehaviour {

	public void onClick() {
		Rules.Instance.ClearAll ();
	}
}
