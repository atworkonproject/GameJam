using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public SelectedTileController SelectedTileC;
	void Start () {
		SelectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}
	
	void Update () {
		if (Input.GetMouseButtonUp(0))//left mouse pressed
		{
			SelectedTileC.Input_MouseLeftDown();
		}
	}
}
