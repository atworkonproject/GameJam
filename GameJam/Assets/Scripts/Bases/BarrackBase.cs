using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackBase : BaseBaseClass {
    float timeToSpawn;
    //public static float spawnEverySec = 5.0f;//moved to config Controller
    //public static int COST_FOR_SOLDIER = 5;

    public GameObject Soldier01prefab;

	public DamageBubbleController DamageBubblC;

	// Use this for initialization
	void Awake () {
        timeToSpawn = 0f;
		DamageBubblC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
	}

	override public void Init2()
    {
        GetComponent<SpriteRenderer>().sprite = (owner.fallen) ? DevilBase : AngelBase;
        HP = getMaxHP();
    }

    override protected int getMaxHP() { return ConfigController.Config.BarracksMaxHP; }

    // Update is called once per frame
    void Update () {
        if (getHP() <= 0)
            return;

        timeToSpawn += Time.deltaTime;
		if (timeToSpawn >= ConfigController.Config.BarrackSpawnEverySec)
		{
			if (owner.Credits >= ConfigController.Config.CostForSoldier)
			{
				timeToSpawn = 0;
				GameObject soldier = Instantiate(Soldier01prefab, this.transform.position + new Vector3(0, 0, -2), Quaternion.identity, GameObject.FindGameObjectWithTag("SOLDIERS").transform);
                Soldier01Controller ctrl = soldier.GetComponent<Soldier01Controller>();
                ctrl.Init(owner);
                gameController.soldiers.Add(ctrl);
                SFXController.PlaySound(SOUNDS.SPAWN);
				owner.Credits -= ConfigController.Config.CostForSoldier;
                if(owner.amIPlayer)
                    DamageBubblC.CreateDamageBubble(this.transform.position, ConfigController.Config.CostForSoldier, false, true);
			}
			else
            {
                if(owner.amIPlayer)
                    UIController.DisplayInfoForPlayer0("no money for a new soldier");
                else
                    UIController.DisplayInfoForPlayer1("no money for a new soldier");

            }
        }
    }

}
