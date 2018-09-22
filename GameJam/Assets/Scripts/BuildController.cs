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
	public SelectedTileController selectedTileC;
	public BaseArrayController baseArrayC;

    public static int FARM_COST = 30;
    public static int BARRACKS_COST = 20;

    gameplayRecorder playerRec, enemyRec;

    float levelTimeElapsed;

    void Start () {
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		baseArrayC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BaseArrayController>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;
        levelTimeElapsed = 0;

        playerRec = new gameplayRecorder();
        enemyRec = new gameplayRecorder();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("r"))
            CleanAll();
	}

	public void BuildBarracks()
	{
		if (UIController.player_Credits > 20)
		{
			if (selectedTileC.DisplayedSelectedTile.isActiveAndEnabled)
			{
				if (BaseArrayController.GetBase(selectedTileC.DisplayedSelectedTile.MyIndexes) == BaseArrayController.NoBase)//if is not occupied by another building
				{
					GameObject go = Instantiate(BarracksBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
					Vector2 basePosition = selectedTileC.DisplayedSelectedTile.transform.position;
					go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
					BaseBaseClass b = go.GetComponent<BaseBaseClass>();
					b.MyIndexes = selectedTileC.DisplayedSelectedTile.MyIndexes;
					BaseArrayController.PutBase(selectedTileC.DisplayedSelectedTile.MyIndexes, b);
					Bases.PlayerBarracksStatic.Add((BarrackBase)b);

                    UIController.player_Credits -= BuildController.BARRACKS_COST;
                    playerRec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_BARRACKS_01, levelTimeElapsed, b.MyIndexes);
                }
                else
					Debug.Log("place occupied");
			}
			else
				Debug.Log("no selected tile");
		}
		else
			Debug.Log("not enough credits");
	}

	public void BuildFarm()
	{
		if (UIController.player_Credits > 30)
		{
			
			if (selectedTileC.DisplayedSelectedTile.isActiveAndEnabled)
			{
				if (BaseArrayController.GetBase(selectedTileC.DisplayedSelectedTile.MyIndexes) == BaseArrayController.NoBase)//if is not occupied by another building
				{
                    GameObject go = Instantiate(FarmBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
					Vector2 basePosition = selectedTileC.DisplayedSelectedTile.transform.position;
					go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
					BaseBaseClass b = go.GetComponent<BaseBaseClass>();
					BaseArrayController.PutBase(selectedTileC.DisplayedSelectedTile.MyIndexes, b);
					b.MyIndexes = selectedTileC.DisplayedSelectedTile.MyIndexes;
					Bases.PlayerFarmsStatic.Add((FarmBase)b);

                    UIController.player_Credits -= BuildController.FARM_COST;
                    playerRec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_FARM, levelTimeElapsed, b.MyIndexes);
                }
                else
					Debug.Log("place occupied");
			}
			else
				Debug.Log("no selected tile");
		}
		else
			Debug.Log("not enough credits");
	}

	[Serializable]
	public class BaseListRow
	{
		public List<BaseBaseClass> row;
	}

    public void CleanAll()
    {
        baseArrayC.DestroyAllBases();
        Bases.DestroyAllBases();
    }
}
