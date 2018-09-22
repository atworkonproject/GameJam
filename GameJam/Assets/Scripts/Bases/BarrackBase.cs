using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackBase : BaseBaseClass {
    float timeToSpawn;
    //public static float spawnEverySec = 5.0f;//moved to config Controller
    //public static int COST_FOR_SOLDIER = 5;

    public GameObject Soldier01prefab;

    public Sprite DevilBase;
    public Sprite AngelBase;

	public DamageBubbleController DamageBubblC;

	// Use this for initialization
	void Awake () {
        timeToSpawn = 0f;
		DamageBubblC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();

	}

	override public void Init2()
    {
        GetComponent<SpriteRenderer>().sprite = fallen ? DevilBase : AngelBase;
		MaxHP = ConfigController.Config.BarracksHP;
        HP = MaxHP;
	}

	// Update is called once per frame
	void Update () {
        timeToSpawn += Time.deltaTime;
		if (timeToSpawn >= ConfigController.Config.BarrackSpawnEverySec)
		{
			if (gameController.playerData.Credits >= ConfigController.Config.CostForSoldier)
			{
				timeToSpawn = 0;
				GameObject soldier = Instantiate(Soldier01prefab, this.transform.position + new Vector3(0, 0, -2), Quaternion.identity, GameObject.FindGameObjectWithTag("SOLDIERS").transform);
				soldier.GetComponent<SoldierController>().Fallen = fallen;
				gameController.playerData.Credits -= ConfigController.Config.CostForSoldier;
				if(this.fallen == gameController.playerData.fallen)
					DamageBubblC.CreateDamageBubble(this.transform.position, ConfigController.Config.CostForSoldier);
			}
			else
				UIController.DisplayInfoForPlayer1("no money for a new soldier");
		}
    }

}
