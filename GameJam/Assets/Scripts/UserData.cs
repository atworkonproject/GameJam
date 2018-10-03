using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public List<FarmBase> Farms;
    public List<BarrackBase01> Barracks01;
    public List<BarrackBase03> Barracks03;

    public float Credits;
    public bool fallen; //god or evil
    public bool amIPlayer;//or Ai

    public gameplayRecorder rec;

    public float income, outcome;//calculate cash. for ai

    public void NewGame(bool isFallen, bool isPlayer)
    {
        Farms = new List<FarmBase>();
        Barracks01 = new List<BarrackBase01>();
        Barracks03 = new List<BarrackBase03>();

        rec = new gameplayRecorder();

        fallen = isFallen;
        amIPlayer = isPlayer;

        StartLevel();
    }

    public void NextLevel()
    {
        //gamecontroller deletes all bases

        //swap fallen and not falle
        fallen = !fallen;

        StartLevel();
    }

    public void StartLevel()
    {
        Credits = ConfigController.Config.FarmBuyCost * 2;//cash for the first farm!

        //without recording it, put a farm around center of my part of map
        Vector2Int firstFarmPos;
        if (amIPlayer)
            firstFarmPos = new Vector2Int(BaseArrayController.mapSize.x / 2, BaseArrayController.mapSize.y / 8);
        else
            firstFarmPos = new Vector2Int(BaseArrayController.mapSize.x / 2, BaseArrayController.mapSize.y * 7 / 8);
        //we can use this because AI version of build farm don't record and don't use  selection base - cursor
        GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>().Build(this, BASE_ID.FARM, firstFarmPos, true);

        Credits = ConfigController.Config.startPlayerCredits;//after creating first farm!
    }

    public void manualUpdate()
    {
		//check if all hp of bases are > 0 if yes game over
        float creditIncrement = 0;
		foreach (FarmBase farm in Farms)
		{
			//creditIncrement += ConfigController.Config.FarmEarnAfterPeriod * Time.deltaTime;
			if (Time.timeSinceLevelLoad - farm.LastTimeFarmEarned > ConfigController.Config.FarmEarnPeriod)
			{
				creditIncrement += ConfigController.Config.FarmEarn;
				if (farm.owner.amIPlayer)
					GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>().CreateDamageBubble(
						    farm.transform.position, ConfigController.Config.FarmEarn, true, true);
				farm.LastTimeFarmEarned = Time.timeSinceLevelLoad;
			}
		}
        Credits += creditIncrement;


        income = ConfigController.Config.FarmEarn / ConfigController.Config.FarmEarnPeriod * Farms.Count;
        outcome = ConfigController.Config.CostForSoldier01 / ConfigController.Config.Barrack01SpawnEverySec * Barracks01.Count;
        outcome = ConfigController.Config.CostForSoldier03 / ConfigController.Config.Barrack03SpawnEverySec * Barracks03.Count;

        Credits = Mathf.Clamp(Credits, 0.0f, ConfigController.Config.maxPlayerCredits);
    }

    //for gamecontroller only
    public void DestroyAllBases()
    {
        foreach (var i in Farms)
            Destroy(i.gameObject);
        foreach (var i in Barracks01)
            Destroy(i.gameObject);
        foreach (var i in Barracks03)
            Destroy(i.gameObject);

        Farms.Clear();
        Barracks01.Clear();
        Barracks03.Clear();
    }
}
