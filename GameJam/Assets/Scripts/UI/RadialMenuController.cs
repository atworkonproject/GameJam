using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuController : MonoBehaviour {
	public SelectedTileController SelectedTileC;
	public BuildController buildC;
	public Camera MainCamera;

	// Use this for initialization
	void Start () {
		SelectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		buildC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BuildController>(true);
		MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		this.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OnFarmClicked()
	{
		buildC.BuildFarmPlayer(gameController.playerData);
		this.gameObject.SetActive(false);
	}

	public void OnBarrackClicked()
	{
		buildC.BuildBarracksPlayer(gameController.playerData);
		this.gameObject.SetActive(false);
	}

	public void ShowMenuOnClick(Vector2 ScreenPosition)
	{
		this.gameObject.transform.position = Input.mousePosition;
		this.gameObject.SetActive(true);
	}
}
