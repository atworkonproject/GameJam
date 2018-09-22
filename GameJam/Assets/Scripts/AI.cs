using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI
{
    BuildController bc;
    UserData AIData;
    //restart all AI every new level
    public void Init(UserData ai)
    {
        AIData = ai;
        bc = GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>();
    }

    public void NextLevel()
    {

    }

    public void Update()
    {
        List<gameplayRecorder.Action> actions = AIData.rec.GetActionsToDo(gameController.timeElapsed);

        foreach(var a in actions)
        {
            switch (a.type)
            {
                case gameplayRecorder.ACTION_TYPE.ADD_FARM:
                    bc.BuildFarmAI(AIData, a.positionIndex);
                    break;
                case gameplayRecorder.ACTION_TYPE.ADD_BARRACKS_01:
                    bc.BuildBarracksAI(AIData, a.positionIndex);
                    break;
                case gameplayRecorder.ACTION_TYPE._COUNT:
                default:
                    break;
            }
        }
    }
}