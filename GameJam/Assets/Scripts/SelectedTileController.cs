using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileController : MonoBehaviour {
	[Header("to link")]
	public GameObject SelectedTilePrefab;
	[Header("other")]
	public GameObject DisplayedSelectedTile;
	public static int DisplayedTileIndexX;
	public static int DisplayedTileIndexY;
	public Sprite DisplayedSelectedTileSprite;
	public Camera MainCamera;
	public Sprite BackgroundSprite;
	//public float SelectedTileWidth;//the bigger the selected size bigger

	void Start () {
		DisplayedSelectedTile = null;
		MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
		//SelectedTileWidth = 128.0f;

		DisplayedSelectedTile = Instantiate(SelectedTilePrefab);
		DisplayedSelectedTileSprite = DisplayedSelectedTile.GetComponent<SpriteRenderer>().sprite;
		DisplayedSelectedTile.SetActive(false);//hide
	}

	public void DisplayTile(Vector2 clickPosition)
	{
		float selectedTileWidth = DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = DisplayedSelectedTileSprite.bounds.size.y;

		float xIndex = (clickPosition.x - BackgroundSprite.bounds.min.x) / selectedTileWidth;//width from left edge of backgroundSprite to click, we divide it by width of selectedTile
		SelectedTileController.DisplayedTileIndexX = Mathf.RoundToInt(xIndex);
		float resultingX = BackgroundSprite.bounds.min.x + (DisplayedTileIndexX * selectedTileWidth);

		float yIndex = (clickPosition.y - BackgroundSprite.bounds.min.y) / selectedTileHeight;
		SelectedTileController.DisplayedTileIndexY = Mathf.RoundToInt(yIndex);
		float resultingY = BackgroundSprite.bounds.min.y + (DisplayedTileIndexY * selectedTileHeight);

		DisplayedSelectedTile.transform.position = new Vector2(resultingX, resultingY);
		DisplayedSelectedTile.SetActive(true);//show
	}

	public void Input_MouseLeftDown()
	{
		DisplayTile(MainCamera.ScreenToWorldPoint(Input.mousePosition));
	}
}
