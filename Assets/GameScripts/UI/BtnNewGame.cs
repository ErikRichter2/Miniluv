using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnNewGame : MonoBehaviour {

	public void onClick() {
		GameModel.Instance.DeleteSave ();
		GameModel.Instance.Init ();
		SceneManager.LoadScene ("Main");
	}

}
