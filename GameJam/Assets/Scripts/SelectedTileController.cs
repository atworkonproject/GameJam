using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileController : MonoBehaviour {
	[Header("to link")]
	public GameObject SelectedTilePrefab;
	[Header("other")]
	public GameObject DisplayedSelectedTile;
	public Camera MainCam;
	void Start () {
		DisplayedSelectedTile = null;
		MainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
	}
	
	void Update () {
		
	}

	public void DisplayTile(Vector2 position)
	{
		if (DisplayedSelectedTile == null)
		{
			GameObject newTile = Instantiate(SelectedTilePrefab);
			newTile.transform.position = position;
			DisplayedSelectedTile = newTile;
		}else
		{
			DisplayedSelectedTile.transform.position = position;
		}
	}

	public void MouseLeftDown()
	{
		
		DisplayTile(MainCam.ScreenToWorldPoint(Input.mousePosition));
	}
}
