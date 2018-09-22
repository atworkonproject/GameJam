using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public List<FarmBase> Farms;
    public List<FarmBase> FarmsDBG;
    public List<BarrackBase> Barracks;
    public List<BarrackBase> BarracksDBG;

    public float Credits;
    public bool fallen; //god or evil
    public bool amIPlayer;//or Ai

    public gameplayRecorder rec;

    public float income, outcome;//calculate cash. for ai

    public void NewGame(bool isFallen, bool isPlayer)
    {
        Farms = new List<FarmBase>();
        Barracks = new List<BarrackBase>();

        FarmsDBG = Farms;
        BarracksDBG = Barracks;

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
            firstFarmPos = new Vector2Int(BaseArrayController.mapSize.x / 2, BaseArrayController.mapSize.y / 4);
        else
            firstFarmPos = new Vector2Int(BaseArrayController.mapSize.x / 2, BaseArrayController.mapSize.y * 3 / 4);
        //we can use this because AI version of build farm don't record and don't use  selection base - cursor
        GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>().BuildFarmAI(this, firstFarmPos);

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
				if (farm.fallen == gameController.playerData.fallen)
				{
					GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>().CreateDamageBubble(
						farm.transform.position, ConfigController.Config.FarmEarn, true);
				}
				farm.LastTimeFarmEarned = Time.timeSinceLevelLoad;
			}
		}
        Credits += creditIncrement;


        income = ConfigController.Config.FarmEarn / ConfigController.Config.FarmEarnPeriod * Farms.Count;
        outcome = ConfigController.Config.CostForSoldier / ConfigController.Config.BarrackSpawnEverySec * Barracks.Count;

        Credits = Mathf.Clamp(gameController.playerData.Credits, 0.0f, ConfigController.Config.maxPlayerCredits);
    }

    //for gamecontroller only
    public void DestroyAllBases()
    {
        foreach (var i in Farms)
            Destroy(i.gameObject);
        foreach (var i in Barracks)
            Destroy(i.gameObject);

        Farms.Clear();
        Barracks.Clear();
    }
}
