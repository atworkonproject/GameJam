using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArrayController : MonoBehaviour {
	public static List<BaseListRow> GlobalBaseArray;//two dimmensional
	public List<BaseListRow> GlobalBaseArrayToView_X;//to view in iinspector
	public static BaseBaseClass NoBase;

	public static BaseBaseClass EmptyClass;
	SelectedTileController selectedTileC;
	BuildController baseC;

	void Start () {
		baseC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BuildController>();
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();

		//initialize
		//if baseArray is initialized before BuildController and selectedTileCotnroller, BackgroundSprite souldn't be initialized
		if (baseC.BackgroundSprite == null)
			baseC.BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
		if(selectedTileC.DisplayedSelectedTile == null)
		{
			selectedTileC.DisplayedSelectedTile = Instantiate(selectedTileC.SelectedTilePrefab);
			selectedTileC.DisplayedSelectedTileSprite = selectedTileC.DisplayedSelectedTile.GetComponent<SpriteRenderer>().sprite;
		}

		int maxIndexX = Mathf.CeilToInt(baseC.BackgroundSprite.bounds.size.x / selectedTileC.DisplayedSelectedTileSprite.bounds.size.x);
		int maxIndexY = Mathf.CeilToInt(baseC.BackgroundSprite.bounds.size.y / selectedTileC.DisplayedSelectedTileSprite.bounds.size.y);

		BaseArrayController.GlobalBaseArray = new List<BaseListRow>();
		for (int i = 0; i <= maxIndexX; i++)
		{
			BaseArrayController.GlobalBaseArray.Add(new BaseListRow());
			BaseArrayController.GlobalBaseArray[i].row_Y = new List<BaseBaseClass>();
			for (int j = 0; j <= maxIndexY; j++)
			{
				BaseArrayController.GlobalBaseArray[i].row_Y.Add(new BaseBaseClass());
				BaseArrayController.GlobalBaseArray[i].row_Y[j] = BaseArrayController.NoBase;
			}
		}
		GlobalBaseArrayToView_X = BaseArrayController.GlobalBaseArray;
	}

	public Vector2Int GetIndexesFromWorldPosition(Vector2 position, out Vector2 returnedWorldIndexedPosition)
	{
		Vector2Int returnedIndexes;
		float selectedTileWidth = selectedTileC.DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = selectedTileC.DisplayedSelectedTileSprite.bounds.size.y;

		position.x -= (selectedTileWidth / 2.0f);//na chama zeby zaczynal od poczatku nie od srodka BackgroundSprite
		position.y -= (selectedTileHeight / 2.0f);

		float xIndex = (position.x - selectedTileC.BackgroundSprite.bounds.min.x) / selectedTileWidth;//width from left edge of backgroundSprite to click, we divide it by width of selectedTile
		int returnedIndexX = Mathf.RoundToInt(xIndex);

		float yIndex = (position.y - selectedTileC.BackgroundSprite.bounds.min.y) / selectedTileHeight;
		int returnedIndexY = Mathf.RoundToInt(yIndex);

		returnedIndexes = new Vector2Int(returnedIndexX, returnedIndexY);
		returnedWorldIndexedPosition = getWorldPositionForIndexes(returnedIndexes);

		return returnedIndexes;
	}

	public Vector2 getWorldPositionForIndexes(Vector2Int indexes)
	{
		float selectedTileWidth = selectedTileC.DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = selectedTileC.DisplayedSelectedTileSprite.bounds.size.y;

		float worldX = selectedTileC.BackgroundSprite.bounds.min.x + (indexes.x * selectedTileWidth) + (selectedTileWidth/2.0f);//(selectedTileWidth/2.0f) na chama zeby zaczynal od poczatku BackgroundSprite nie od srodka
		float worldY = selectedTileC.BackgroundSprite.bounds.min.y + (indexes.y * selectedTileHeight) + (selectedTileHeight / 2.0f);


		return new Vector2(worldX, worldY);
	}

	public static BaseBaseClass GetBase(Vector2Int indexes)
	{
		return BaseArrayController.GlobalBaseArray[indexes.x].row_Y[indexes.y];

	}

	public static void PutBase(Vector2Int indexes, BaseBaseClass baseToPut)
	{
		BaseArrayController.GlobalBaseArray[indexes.x].row_Y[indexes.y] = baseToPut;
	}
	public void GetXIndexOfPositionX(float positionX)
	{

	}

	[Serializable]
	public class BaseListRow
	{
		public List<BaseBaseClass> row_Y;
	}
}
