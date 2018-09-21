using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileController : MonoBehaviour {
	[Header("to link")]
	public GameObject SelectedTilePrefab;
	[Header("other")]
	public GameObject DisplayedSelectedTile;
	public Sprite DisplayedSelectedTileSprite;
	public Camera MainCamera;
	public Sprite BackgroundSprite;
	public float SelectedTileWidth;//the bigger the selected size bigger

	void Start () {
		DisplayedSelectedTile = null;
		MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
		SelectedTileWidth = 128.0f;
	}

	public void DisplayTile(Vector2 clickPosition)
	{
		if (DisplayedSelectedTile == null)
		{
			GameObject newTile = Instantiate(SelectedTilePrefab);
			DisplayedSelectedTile = newTile;
			DisplayedSelectedTileSprite = DisplayedSelectedTile.GetComponent<SpriteRenderer>().sprite;
		}

		float selectedTileWidth = DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = DisplayedSelectedTileSprite.bounds.size.y;

		float xIndex = (clickPosition.x - BackgroundSprite.bounds.min.x) / selectedTileWidth;//width from left edge of backgroundSprite to click, we divide it by width of selectedTile
		float resultingX = BackgroundSprite.bounds.min.x + (Mathf.Round(xIndex) * selectedTileWidth);

		float yIndex = (clickPosition.y - BackgroundSprite.bounds.min.y) / selectedTileHeight;
		float resultingY = BackgroundSprite.bounds.min.y + (Mathf.Round(yIndex) * selectedTileHeight);

		DisplayedSelectedTile.transform.position = new Vector2(resultingX, resultingY);
	}

	public void MouseLeftDown()
	{
		
		DisplayTile(MainCamera.ScreenToWorldPoint(Input.mousePosition));
	}
}
