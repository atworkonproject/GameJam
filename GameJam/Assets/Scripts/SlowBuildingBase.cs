using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBuildingBase : MonoBehaviour
{
	public float SecondsBuildingLeft;
	private float SecondsBuildingTotal;
	public SpriteMask Mask;
	private float oneSecondCounter;
	public Sprite AngelBase;
	public Sprite DevilBase;
	// Use this for initialization
	void Start()
	{
        SecondsBuildingTotal = ConfigController.Config.BUILDING_TIME;
		SecondsBuildingLeft = SecondsBuildingTotal;
		Mask = GetComponentInChildren<SpriteMask>(true);
		oneSecondCounter = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		SecondsBuildingLeft -= Time.deltaTime;
		oneSecondCounter += Time.deltaTime;
		if (oneSecondCounter >= 1.0f)//update every second
		{
			oneSecondCounter = 0f;
			if (SecondsBuildingLeft <= 0.0f)
			{
				FinishBuilding();
			}
			else
			{
				Mask.transform.localScale = new Vector3(
					Mask.transform.localScale.x,
					(SecondsBuildingLeft * 1.0f) / SecondsBuildingTotal,
					Mask.transform.localScale.z
				);
			}
		}
	}

	//data for creating new base
	Vector3 baseposition;
	GameObject prefabGO;
	//SelectedTileController selectedTileC;
	UserData builder;
	Vector2 basePosition;
	Vector2Int DisplayedSelectedTile_MyIndexes;
    bool record;//is the action recorded for later use

    //SelectedTileController selectedTileContr, 
    public void Init(Vector2Int pos,  GameObject prefabGO, UserData builder, bool _record)
	{
		this.prefabGO = prefabGO;
		//this.selectedTileC = selectedTileContr;
		this.builder = builder;
        record = _record;
        basePosition = BaseArrayController.getWorldPositionForIndexes(pos);
        DisplayedSelectedTile_MyIndexes = pos;

        this.gameObject.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, BaseArrayController.SlowBuildingBaseStatic);
		GetComponent<SpriteRenderer>().sprite = (builder.fallen) ? DevilBase : AngelBase;
	}

    public void FinishBuilding()
	{
		GameObject go = Instantiate(prefabGO, GameObject.FindGameObjectWithTag("BASES").transform);
		go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		BaseBaseClass b = go.GetComponent<BaseBaseClass>();
		b.MyIndexes = DisplayedSelectedTile_MyIndexes;
		if (BaseArrayController.GetBase(DisplayedSelectedTile_MyIndexes) == BaseArrayController.SlowBuildingBaseStatic)//destroy slowBuildingBase
			BaseArrayController.RemoveBase(DisplayedSelectedTile_MyIndexes);
		else
			Debug.Log("nobuilding base a powinna byc");
		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, BaseArrayController.NoBaseStatic);

		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, b);

		if (b is BarrackBase01)
		{
			builder.Barracks01.Add((BarrackBase01)b);
            if(record)
            builder.rec.AddAction(ACTION_ID.ADD_BARRACKS_01, gameController.timeElapsed, b.MyIndexes);
        }
        else if (b is BarrackBase03)
        {
            builder.Barracks03.Add((BarrackBase03)b);
            if (record)
                builder.rec.AddAction(ACTION_ID.ADD_BARRACKS_03, gameController.timeElapsed, b.MyIndexes);
        }
        else if (b is FarmBase)
		{
			builder.Farms.Add((FarmBase)b);
            if(record)
            builder.rec.AddAction(ACTION_ID.ADD_FARM, gameController.timeElapsed, b.MyIndexes);
        }
        b.Init(builder, b.MyIndexes);

        if (builder == gameController.playerData)
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().playerAliveBases++;
        else
            GameObject.FindWithTag("BuildController").GetComponent<BuildController>().AIaliveBases++;

        //this.gameObject.SetActive(false);//hide
        Destroy(this.gameObject);
	}
}
