﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoDesk : MonoBehaviour, ITaskable {

	public EntityQueue GetEntityQueue() {
		return GetComponentInChildren<EntityQueue> ();
	}

	public void OnMouseDown() {
		if (BasePopup.IsPopupActive() == false) {
			PopupShowRules popup = BasePopup.GetPopup<PopupShowRules> ();
			popup.ShowStamp (0);
		}
	}

	public int GetTaskDuration() {
		return int.Parse(DefinitionsLoader.configDefinition.GetItem(ConfigDefinition.INFOPOINT_TIME).Value);
	}


}