using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuyBaseButton : MonoBehaviour {

	BuildController buildC;
	SelectedTileController selectedTileC;
    public BASE_ID id;

	public void Start()
	{
		buildC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BuildController>();
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}

	public void OnClickedMe()
	{
		buildC.BuildPlayer(gameController.playerData, id);
    }
}
