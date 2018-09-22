using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBarracksButton : MonoBehaviour {

	public BuildController buildC;
	public SelectedTileController selectedTileC;
	public void Start()
	{
		buildC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BuildController>();
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}

	public void OnClickedMe()
	{
		buildC.BuildBarracksPlayer(gameController.playerData);
	}
}
