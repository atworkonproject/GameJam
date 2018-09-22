using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArrayController : MonoBehaviour {
	public static List<BaseListRow> GlobalBaseArray;//two dimmensional
	public List<BaseListRow> GlobalBaseArrayToView;//to view in iinspector
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
			BaseArrayController.GlobalBaseArray[i].row = new List<BaseBaseClass>();
			for (int j = 0; j <= maxIndexY; j++)
			{
				BaseArrayController.GlobalBaseArray[i].row.Add(new BaseBaseClass());
				BaseArrayController.GlobalBaseArray[i].row[j] = BaseArrayController.NoBase;
			}
		}
		GlobalBaseArrayToView = BaseArrayController.GlobalBaseArray;
	}

	public Vector2Int GetIndexesFromWorldPosition(Vector2 position, out Vector2 worldIndexedPosition)
	{
		Vector2Int returnedIndexes;
		float selectedTileWidth = selectedTileC.DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = selectedTileC.DisplayedSelectedTileSprite.bounds.size.y;


		float xIndex = (position.x - selectedTileC.BackgroundSprite.bounds.min.x) / selectedTileWidth;//width from left edge of backgroundSprite to click, we divide it by width of selectedTile
		returnedIndexes = new Vector2Int(Mathf.RoundToInt(xIndex), 0);
		//float worldIndexedX = selectedTileC.BackgroundSprite.bounds.min.x + (returnedIndexes.x * selectedTileWidth);

		float yIndex = (position.y - selectedTileC.BackgroundSprite.bounds.min.y) / selectedTileHeight;
		returnedIndexes = new Vector2Int(returnedIndexes.x, Mathf.RoundToInt(yIndex));
		//float worldIndexedY = selectedTileC.BackgroundSprite.bounds.min.y + (returnedIndexes.y * selectedTileHeight);

		worldIndexedPosition = getWorldPositionForIndexes(returnedIndexes);

		return returnedIndexes;
	}

	public Vector2 getWorldPositionForIndexes(Vector2Int indexes)
	{
		float selectedTileWidth = selectedTileC.DisplayedSelectedTileSprite.bounds.size.x;
		float selectedTileHeight = selectedTileC.DisplayedSelectedTileSprite.bounds.size.y;

		float worldIndexedX = selectedTileC.BackgroundSprite.bounds.min.x + (indexes.x * selectedTileWidth);
		float worldIndexedY = selectedTileC.BackgroundSprite.bounds.min.y + (indexes.y * selectedTileHeight);

		return new Vector2(worldIndexedX, worldIndexedY);
	}

	public static BaseBaseClass GetBase(Vector2Int indexes)
	{
		return BaseArrayController.GlobalBaseArray[indexes.x].row[indexes.y];

	}

	public static void PutBase(Vector2Int indexes, BaseBaseClass baseToPut)
	{
		BaseArrayController.GlobalBaseArray[indexes.x].row[indexes.y] = baseToPut;
	}
	public void GetXIndexOfPositionX(float positionX)
	{

	}

	[Serializable]
	public class BaseListRow
	{
		public List<BaseBaseClass> row;
	}
}
