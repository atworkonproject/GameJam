using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	[Header("to link")]
	public BarrackBase01 BarracksBasePrefab;
	public FarmBase FarmBasePrefab;
	public SlowBuildingBase BarracksSlowBuildingBasePrefab;
	public FarmBase FarmSlowBuildingBasePrefab;
	[Header("other")]
	public Sprite BackgroundSprite;
	public SelectedTileController selectedTileC;
	public BaseArrayController baseArrayC;
	public DamageBubbleController DamageBubbleC;

    public GameObject[] basePrefabs;//like in enum BASE_ID but - slow1, slow2, normal1, normal2..

    public int playerAliveBases;
	public int AIaliveBases;

	void Start () {
        selectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
		baseArrayC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<BaseArrayController>();
		DamageBubbleC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
		BackgroundSprite = GameObject.FindWithTag("BackgroundSprite").GetComponent<SpriteRenderer>().sprite;

        playerAliveBases = AIaliveBases = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildPlayer(UserData builder, BASE_ID id)
    {
        if (!selectedTileC.DisplayedSelectedTile.isActiveAndEnabled)
        {
            UIController.DisplayInfoForPlayer0("Place for new building is not selected");
            return;
        }

        Vector2Int pos = selectedTileC.DisplayedSelectedTile.MyIndexes;
        Build(builder, id, pos);

        DamageBubbleC.CreateDamageBubble(selectedTileC.DisplayedSelectedTile.transform.position,
            ConfigController.Config.Barracks01BuyCost, false, true);

        selectedTileC.HideSelectionTile();
    }
    public void BuildAI(UserData builder, BASE_ID id, Vector2Int pos)
    {
        Build(builder, id, pos);
    }
    //with recording (player uses this)
    public void Build(UserData builder, BASE_ID id, Vector2Int pos, bool isStartGameBuilding = false)
    {
        //check correct pos
        if (pos.x < 0 || pos.y < 0)
        {
            Debug.LogError("random position for new " + id.ToString() + "is not ok");
            return;
        }

        //check position if free
        if (BaseArrayController.GetBase(pos) != BaseArrayController.NoBaseStatic)
        {
            UIController.DisplayInfoForPlayer0("The place is occupied");
            return;
        }

        //check credit
        float creditsNeeded = 100;
        switch (id)
        {
            case BASE_ID.FARM:
                creditsNeeded = ConfigController.Config.FarmBuyCost;
                break;
            case BASE_ID.BARRACKS_01:
                creditsNeeded = ConfigController.Config.Barracks01BuyCost;
                break;
            case BASE_ID.BARRACKS_02:
                creditsNeeded = ConfigController.Config.Barracks02BuyCost;
                break;
            case BASE_ID.BARRACKS_03:
            case BASE_ID._COUNT:
            default:
                creditsNeeded = ConfigController.Config.Barracks03BuyCost;
                break;
        }
        if (builder.Credits < creditsNeeded)
        {
            UIController.DisplayInfoForPlayer0("Not enough $");
            return;
        }
        
		GameObject go = Instantiate(GetPrefab(id, true), GameObject.FindGameObjectWithTag("BASES").transform);

        if (!go.GetComponent<SlowBuildingBase>())
        {
            Debug.LogError("error no slow building");
            return;
        }

        go.GetComponent<SlowBuildingBase>().Init(pos, GetPrefab(id, false),
            builder, !isStartGameBuilding);//do not record initial buildin creation
			

        if(isStartGameBuilding)
        {
            //instant finish
            go.GetComponent<SlowBuildingBase>().FinishBuilding();
        }

        builder.Credits -= creditsNeeded;
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

    private GameObject GetPrefab(BASE_ID id, bool slow)
    {
        int ind = (slow ? (int)(BASE_ID._COUNT) + ((int)id) : ((int)id));
        if(ind >= basePrefabs.Length)
        {
            Debug.LogError("error - id");
            return basePrefabs[0];
        }
        return basePrefabs[ind];
    }
}
