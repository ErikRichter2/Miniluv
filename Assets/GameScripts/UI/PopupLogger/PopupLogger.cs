using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PopupLogger : BasePopup {

	public Text LogTextArea;

	public void ShowMessage(string message) {		
		this.ShowPopup ();

		this.LogTextArea.text = message;
	}

	public void OnConfirm() {
		HidePopup ();
		GameModel.Instance.Init ();
		GameModel.Instance.Load ();
		SceneManager.LoadScene ("Main");
	}


}
