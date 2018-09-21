﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFarmButton : MonoBehaviour {

	public BuildController buildC;
	public SelectedTileController selectedTileC;
	public void Start()
	{
		buildC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BuildController>();
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}

	public void OnClickedMe()
	{
		if (selectedTileC.DisplayedSelectedTile.transform.position != null)
			buildC.BuildFarm(selectedTileC.DisplayedSelectedTile.transform.position);
		else
			Debug.Log("no selected tile");
	}
}
