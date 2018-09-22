using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	[Header("to link")]
	public BarrackBase BarracksBasePrefab;
	public FarmBase FarmBasePrefab;
	[Header("other")]
	public Sprite BackgroundSprite;
	SelectedTileController selectedTileC;

	public static List<BaseListRow> GlobalBaseArray;//two dimmensional
	public List<BaseListRow> GlobalBaseArrayToView;//to view in iinspector
	public static BaseBaseClass EmptyClass;

	void Start () {
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
		int maxIndexX = Mathf.CeilToInt(BackgroundSprite.bounds.size.x / selectedTileC.DisplayedSelectedTileSprite.bounds.size.x);
		int maxIndexY = Mathf.CeilToInt(BackgroundSprite.bounds.size.y / selectedTileC.DisplayedSelectedTileSprite.bounds.size.y);

		//initialize
		BuildController.GlobalBaseArray = new List<BaseListRow>();
		for (int i = 0; i <= maxIndexX; i++)
		{
			BuildController.GlobalBaseArray.Add(new BaseListRow());
			BuildController.GlobalBaseArray[i].row = new List<BaseBaseClass>();
			for(int j = 0; j <= maxIndexY; j++)
			{
				BuildController.GlobalBaseArray[i].row.Add(new BaseBaseClass());
				BuildController.GlobalBaseArray[i].row[j] = BuildController.EmptyClass;
			}
		}
		GlobalBaseArrayToView = BuildController.GlobalBaseArray;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildBarracks()
	{
		if (selectedTileC.DisplayedSelectedTile != null)
		{
			if (BuildController.GlobalBaseArray[SelectedTileController.DisplayedTileIndexX].row[SelectedTileController.DisplayedTileIndexY] == BuildController.EmptyClass)//if is not occupied by another building
			{
				Vector2 position = selectedTileC.DisplayedSelectedTile.transform.position;
				GameObject go = Instantiate(BarracksBasePrefab.gameObject);
				go.transform.position = new Vector3(position.x, position.y, -1.0f);//-1.0f to be in front of backgroundSprite
				BaseBaseClass b = go.GetComponent<BaseBaseClass>();
				BuildController.GlobalBaseArray[SelectedTileController.DisplayedTileIndexX].row[SelectedTileController.DisplayedTileIndexY] = b;
				PlayerBases.PlayerBarracksStatic.Add((BarrackBase)b);
			}
			else
				Debug.Log("place occupied");
		}
		else
			Debug.Log("no selected tile");
	}
	public void BuildFarm()
	{
		if (selectedTileC.DisplayedSelectedTile != null)
		{
			if (BuildController.GlobalBaseArray[SelectedTileController.DisplayedTileIndexX].row[SelectedTileController.DisplayedTileIndexY] == BuildController.EmptyClass)
			{
				Vector2 position = selectedTileC.DisplayedSelectedTile.transform.position;
				GameObject go = Instantiate(FarmBasePrefab.gameObject);
				go.transform.position = new Vector3(position.x, position.y, -1.0f);//-1.0f to be in front of backgroundSprite
				BaseBaseClass b = go.GetComponent<BaseBaseClass>();
				BuildController.GlobalBaseArray[SelectedTileController.DisplayedTileIndexX].row[SelectedTileController.DisplayedTileIndexY] = b;
				PlayerBases.PlayerFarmsStatic.Add((FarmBase)b);
			}
			else
				Debug.Log("place occupied");
		}
		else
			Debug.Log("no selected tile");
	}

	[Serializable]
	public class BaseListRow
	{
		public List<BaseBaseClass> row;
	}
}
