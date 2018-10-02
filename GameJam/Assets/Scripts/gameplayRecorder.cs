using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameplayRecorder {

    public enum ACTION_TYPE
    {
        ADD_FARM,
        ADD_BARRACKS_01,

        _COUNT
    }
    public class Action
    {
        public ACTION_TYPE type { get; private set; }
        public float time { get; private set; }//counting from level start
        public Vector2Int positionIndex { get; private set; }//position of newly base on the map (optionally)

        public Action(ACTION_TYPE _type, float _time, Vector2Int pos)
        {
            type = _type;
            time = _time;
            positionIndex = pos;
        }

        public void SwapPosition()//swap position to the other side of map
        {
            Vector2Int size = BaseArrayController.mapSize;
            positionIndex = new Vector2Int(size.x - positionIndex.x - 1,
                                            size.y - positionIndex.y - 1);
        }
        public void ChangeTime(float newTime) { time = newTime; }
    }

    List<Action> playerActions = new List<Action>();//chronogically set in the list!

    //return all actions (and removes originals) which should be done in current frame.
    public List<Action> GetActionsToDo(float timeElapsed)//returns all actions which happened before 'timeElapsed ' and removes them from the list
    {
        List<Action> actions = new List<Action>();
        while (playerActions.Count > 0)
        {
            if (playerActions[0].time <= timeElapsed)
            {
                actions.Add(playerActions[0]);
                playerActions.RemoveAt(0);
            }
            else
                break;
        }

        return actions;
    }

    public void AddAction(ACTION_TYPE type, float elapsedTime, Vector2Int pos)
    {
        Action a = new Action(type, elapsedTime, pos);
        playerActions.Add(a);
    }

    public void ResetAll()
    {
        playerActions.Clear();
    }

    public int GetTotalActions() { return playerActions.Count; }

    //changes positions of decisions so now the second side will make actions
    public void SwapSides()
    {
        foreach(Action i in playerActions)
            i.SwapPosition();
    }

    public void TrimInitialDelay(float maxTrim)//if delay before first action is more than 'maxTrim' then make it short, = 'maxTrim'. (if player slowly begins first level)
    {
        if (GetTotalActions() <= 0 || maxTrim < 0)
            return;

        float firstActionTime = playerActions[0].time;

        if (firstActionTime > maxTrim)
        {
            foreach (Action a in playerActions)
                a.ChangeTime(a.time - firstActionTime + maxTrim);
        }
    }
}
