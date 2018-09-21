using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public SelectedTileController SelectedTileC;
	void Start () {
		SelectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}
	
	void Update () {
		if (Input.GetMouseButton(0))//left mouse down
		{
			SelectedTileC.MouseLeftDown();
		}
	}
}
