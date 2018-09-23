using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBuildingMask : MonoBehaviour {
	public float SecondsBuildingLeft;
	private float SecondsBuildingTotal;
	public SpriteMask Mask;
	// Use this for initialization
	void Start () {
		SecondsBuildingTotal = 7.0f;//how much time will it build. todo move to config
		SecondsBuildingLeft = SecondsBuildingTotal;
		Mask = GetComponent<SpriteMask>();
	}
	
	// Update is called once per frame
	void Update () {
		SecondsBuildingLeft -= Time.deltaTime;
		if (Time.timeSinceLevelLoad % 1.0f == 0)//update every second
		{
			if (SecondsBuildingLeft <= 0.0f)
				FinishBuilding();
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

	}

	public void FinishBuilding()
	{
		GameObject go = Instantiate(prefabGO, GameObject.FindGameObjectWithTag("BASES").transform);
		go.transform.position = new Vector3(basePosition.x, basePosition.y, -1.0f);//-1.0f to be in front of backgroundSprite
		BaseBaseClass b = go.GetComponent<BaseBaseClass>();
		b.MyIndexes = DisplayedSelectedTile_MyIndexes;
		BaseArrayController.PutBase(DisplayedSelectedTile_MyIndexes, b);
		builder.Barracks.Add((BarrackBase)b);
		((BarrackBase)b).Init(builder, b.MyIndexes);
	}
}
