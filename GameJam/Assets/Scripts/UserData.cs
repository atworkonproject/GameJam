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

        Credits = ConfigController.Config.startPlayerCredits;
        fallen = isFallen;
        amIPlayer = isPlayer;
    }

    public void NextLevel()
    {
        //gamecontroller deletes all bases
        Credits = ConfigController.Config.startPlayerCredits;

        //swap fallen and not falle
        fallen = !fallen;
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
				GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>().CreateDamageBubble(
					farm.transform.position, ConfigController.Config.FarmEarn, true);
				farm.LastTimeFarmEarned = Time.timeSinceLevelLoad;
			}
		}
        Credits += creditIncrement;


        income = creditIncrement;
        outcome = ConfigController.Config.CostForSoldier / ConfigController.Config.BarrackSpawnEverySec * Barracks.Count * Time.deltaTime;


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
