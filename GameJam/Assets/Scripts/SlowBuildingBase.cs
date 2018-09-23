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
		SecondsBuildingTotal = 7.0f;//how much time will it build. todo move to config
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
	SelectedTileController selectedTileC;
	UserData builder;
	Vector2 basePosition;
	Vector2Int DisplayedSelectedTile_MyIndexes;

	public void Init(GameObject prefabGO, SelectedTileController selectedTileContr, UserData builder)
	{
		this.prefabGO = prefabGO;
		this.selectedTileC = selectedTileContr;
		this.builder = builder;
		basePosition = selectedTileC.DisplayedSelectedTile.transform.position;
		DisplayedSelectedTile_MyIndexes = selectedTileC.DisplayedSelectedTile.MyIndexes;

		this.gameObject.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		GetComponent<SpriteRenderer>().sprite = (builder.fallen) ? DevilBase : AngelBase;

	}

	public void InitForAI(Vector2Int pos, GameObject prefabGO, SelectedTileController selectedTileContr, UserData builder)
	{
		this.prefabGO = prefabGO;
		this.selectedTileC = selectedTileContr;
		this.builder = builder;

		basePosition = BaseArrayController.getWorldPositionForIndexes(pos);
		DisplayedSelectedTile_MyIndexes = pos;
		this.gameObject.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		BaseArrayController.PutBase(pos, BaseArrayController.SlowBuildingBase);
		GetComponent<SpriteRenderer>().sprite = (builder.fallen) ? DevilBase : AngelBase;
	}

	public void FinishBuilding()
	{
		GameObject go = Instantiate(prefabGO, GameObject.FindGameObjectWithTag("BASES").transform);
		go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		BaseBaseClass b = go.GetComponent<BaseBaseClass>();
		b.MyIndexes = DisplayedSelectedTile_MyIndexes;
		if (BaseArrayController.GetBase(DisplayedSelectedTile_MyIndexes) == BaseArrayController.SlowBuildingBase)//destroy slowBuildingBase
			BaseArrayController.RemoveBase(DisplayedSelectedTile_MyIndexes);
		else
			Debug.Log("nobuilding base a powinna byc");
		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, BaseArrayController.NoBase);

		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, b);
		if (b is BarrackBase)
		{
			builder.Barracks.Add((BarrackBase)b);
			((BarrackBase)b).Init(builder, b.MyIndexes);
		}
		else if (b is FarmBase)
		{
			builder.Farms.Add((FarmBase)b);
			((FarmBase)b).Init(builder, b.MyIndexes);
		}
		builder.rec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_BARRACKS_01, gameController.timeElapsed, b.MyIndexes);
		builder.rec.AddAction(gameplayRecorder.ACTION_TYPE.ADD_FARM, gameController.timeElapsed, b.MyIndexes);

		//this.gameObject.SetActive(false);//hide
		Destroy(this.gameObject);
	}
}
