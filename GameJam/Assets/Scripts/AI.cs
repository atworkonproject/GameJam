using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI
{
    BuildController bc;
    UserData AIData;

    float decisionTimer;
    //restart all AI every new level
    public void Init(UserData ai)
    {
        AIData = ai;
        decisionTimer = 0;
        bc = GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>();
    }

    public void NextLevel()
    {
        decisionTimer = 0;
    }

    public void Update()
    {
        //until AI didn't committed all old players moves...
        if (AIData.rec.GetTotalActions() > 0)
        {
            List<gameplayRecorder.Action> actions = AIData.rec.GetActionsToDo(gameController.timeElapsed);

            foreach (var a in actions)
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
        else
        {
            //make an decision every second to slow down
            if(decisionTimer >= ConfigController.Config.AIDecisionEverySec)
            {
                HahahahaEvilPlanOfTheAI();
                decisionTimer = 0;
            }
            decisionTimer += Time.deltaTime;

        }
    }

    private void HahahahaEvilPlanOfTheAI()
    {
        if (AIData.Farms.Count <= 0 ||
            AIData.income < AIData.outcome)
        {
            //try to build a farm
            bc.BuildFarmAI(AIData, GetRandomFreePos());
        }
        else
        {
            bc.BuildBarracksAI(AIData, GetRandomFreePos());
        }
    }

    Vector2Int GetRandomFreePos()//returns (-1,-1) if impossible
    {
        int guard = 1000;

        Vector2Int pos = new Vector2Int(0, 0);
        do
        {
            if (guard-- <= 0)
                return new Vector2Int(-1, -1);
            pos.x = UnityEngine.Random.Range(0, BaseArrayController.mapSize.x);
            pos.y = UnityEngine.Random.Range(BaseArrayController.mapSize.y / 2, BaseArrayController.mapSize.y);
        } while (BaseArrayController.GetBase(pos) != BaseArrayController.NoBase);

        return pos;//free pos
    }
}