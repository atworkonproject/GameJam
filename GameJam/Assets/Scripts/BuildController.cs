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

    public bool playerFallen;

    gameplayRecorder playerRec, enemyRec;

    void Start () {
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		baseArrayC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BaseArrayController>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;

        playerRec = new gameplayRecorder();
        enemyRec = new gameplayRecorder();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
     
    //with recording (player uses this)
	public void BuildBarracksPlayer(UserData builder)
	{
		if (builder.Credits >= ConfigController.Config.BarracksBuyCost)
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
                    builder.Barracks.Add((BarrackBase)b);
                    ((BarrackBase)b).Init(builder.fallen, builder.amIPlayer, b.MyIndexes);

                    builder.Credits -= BuildController.BARRACKS_COST;
					builder.rec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_BARRACKS_01, gameController.timeElapsed, b.MyIndexes);

					selectedTileC.HideSelectionTile();
				}
				else
					UIController.DisplayInfoForPlayer0("place occupied");
			}
			else
				UIController.DisplayInfoForPlayer0("no selected tile");
		}
		else
			UIController.DisplayInfoForPlayer0("not enough credits");
	}
    //no recording (because AI uses this)
    public void BuildBarracksAI(UserData builder, Vector2Int pos)
    {
        if (builder.Credits < ConfigController.Config.BarracksBuyCost)
            return;
        if (BaseArrayController.GetBase(pos) != BaseArrayController.NoBase)
            return;
        if (pos.x < 0 || pos.y < 0)
            return;

        GameObject go = Instantiate(BarracksBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
        Vector2 basePosition = BaseArrayController.getWorldPositionForIndexes(pos);
        go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
        BaseBaseClass b = go.GetComponent<BaseBaseClass>();
        b.MyIndexes = pos;
        BaseArrayController.PutBase(pos, b);
        builder.Barracks.Add((BarrackBase)b);
        ((BarrackBase)b).Init(builder.fallen, builder.amIPlayer, b.MyIndexes);

        builder.Credits -= BuildController.BARRACKS_COST;
	}

	public void BuildFarmPlayer(UserData builder)
	{
		if (gameController.playerData.Credits >= ConfigController.Config.FarmBuyCost)
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
                    builder.Farms.Add((FarmBase)b);
                    ((FarmBase)b).Init(builder.fallen, builder.amIPlayer, b.MyIndexes);

                    builder.Credits -= BuildController.FARM_COST;
                    builder.rec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_FARM, gameController.timeElapsed, b.MyIndexes);

					selectedTileC.HideSelectionTile();
				}
				else
					UIController.DisplayInfoForPlayer0("place occupied");
			}
			else
				UIController.DisplayInfoForPlayer0("no selected tile");
		}
		else
			UIController.DisplayInfoForPlayer0("not enough credits");
	}
    public void BuildFarmAI(UserData builder, Vector2Int pos)
    {
        if (builder.Credits < ConfigController.Config.FarmBuyCost)
            return;
        if (BaseArrayController.GetBase(pos) != BaseArrayController.NoBase)
            return;
        if (pos.x < 0 || pos.y < 0)
            return;

        GameObject go = Instantiate(FarmBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
        Vector2 basePosition = BaseArrayController.getWorldPositionForIndexes(pos);
        go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
        BaseBaseClass b = go.GetComponent<BaseBaseClass>();
        b.MyIndexes = pos;
        BaseArrayController.PutBase(pos, b);
        builder.Farms.Add((FarmBase)b);
        ((FarmBase)b).Init(builder.fallen, builder.amIPlayer, b.MyIndexes);

        builder.Credits -= BuildController.FARM_COST;
	}

	[Serializable]
	public class BaseListRow
	{
		public List<BaseBaseClass> row;
	}

    public void CleanAllBasesOnMap()
    {
        baseArrayC.DestroyAllBases();
    }
}
