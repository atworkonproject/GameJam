using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	[Header("to link")]
	public BarrackBase BarracksBasePrefab;
	public FarmBase FarmBasePrefab;
	public SlowBuildingBase BarracksSlowBuildingBasePrefab;
	public FarmBase FarmSlowBuildingBasePrefab;
	[Header("other")]
	public Sprite BackgroundSprite;
	public SelectedTileController selectedTileC;
	public BaseArrayController baseArrayC;

	public int playerAliveBases;
	public int AIaliveBases;

	void Start () {
		selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		baseArrayC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BaseArrayController>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;

        playerAliveBases = AIaliveBases = 0;
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
				if (BaseArrayController.GetBase(selectedTileC.DisplayedSelectedTile.MyIndexes) == BaseArrayController.NoBaseStatic)//if is not occupied by another building
				{
					GameObject go = Instantiate(BarracksSlowBuildingBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
					go.GetComponent<SlowBuildingBase>().Init(BarracksBasePrefab.gameObject,
						selectedTileC, builder);

                    if (builder.fallen == gameController.playerData.fallen)
                        GameObject.FindWithTag("BuildController").GetComponent<BuildController>().playerAliveBases++;
                    else
                        GameObject.FindWithTag("BuildController").GetComponent<BuildController>().AIaliveBases++;

                    builder.Credits -= ConfigController.Config.BarracksBuyCost;
					SFXController.PlaySound(SOUNDS.PLACE_BUILDING);

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
        if (BaseArrayController.GetBase(pos) != BaseArrayController.NoBaseStatic)
            return;
        if (pos.x < 0 || pos.y < 0)
            return;

		GameObject go = Instantiate(BarracksSlowBuildingBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
		go.GetComponent<SlowBuildingBase>().InitForAI(pos, BarracksBasePrefab.gameObject,
			selectedTileC, builder);

        if (builder.fallen == gameController.playerData.fallen)
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().playerAliveBases++;
        else
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().AIaliveBases++;

        builder.Credits -= ConfigController.Config.BarracksBuyCost;
        SFXController.PlaySound(SOUNDS.PLACE_BUILDING);
	}

	public void BuildFarmPlayer(UserData builder)
	{
		if (gameController.playerData.Credits >= ConfigController.Config.FarmBuyCost)
		{
			
			if (selectedTileC.DisplayedSelectedTile.isActiveAndEnabled)
			{
				if (BaseArrayController.GetBase(selectedTileC.DisplayedSelectedTile.MyIndexes) == BaseArrayController.NoBaseStatic)//if is not occupied by another building
				{
					GameObject go = Instantiate(FarmSlowBuildingBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
					go.GetComponent<SlowBuildingBase>().Init(FarmBasePrefab.gameObject,
						selectedTileC, builder);

                    if (builder.fallen == gameController.playerData.fallen)
                        GameObject.FindWithTag("BuildController").GetComponent<BuildController>().playerAliveBases++;
                    else
                        GameObject.FindWithTag("BuildController").GetComponent<BuildController>().AIaliveBases++;

                    builder.Credits -= ConfigController.Config.FarmBuyCost;
                    SFXController.PlaySound(SOUNDS.PLACE_BUILDING);

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
	

    public void BuildFarmAI(UserData builder, Vector2Int pos, bool isThisFirstFarm = false)
    {
        if (builder.Credits < ConfigController.Config.FarmBuyCost)
            return;
        if (BaseArrayController.GetBase(pos) != BaseArrayController.NoBaseStatic)
            return;
        if (pos.x < 0 || pos.y < 0)
            return;

		GameObject go = Instantiate(FarmSlowBuildingBasePrefab.gameObject, GameObject.FindGameObjectWithTag("BASES").transform);
		go.GetComponent<SlowBuildingBase>().InitForAI(pos, FarmBasePrefab.gameObject,
			selectedTileC, builder);

		if (isThisFirstFarm)//build instantenously
			go.GetComponent<SlowBuildingBase>().FinishBuilding();

        if (builder.fallen == gameController.playerData.fallen)
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().playerAliveBases++;
        else
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().AIaliveBases++;

        builder.Credits -= ConfigController.Config.FarmBuyCost;
        SFXController.PlaySound(SOUNDS.PLACE_BUILDING);
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
