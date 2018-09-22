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
    }

    List<Action> playerActions = new List<Action>();//chronogically set in the list!

    //return all actions (and removes originals) which should be done in current frame.
    public List<ACTION_TYPE> GetActionsToDo(float timeElapsed)//returns all actions which happened before 'timeElapsed ' and removes them from the list
    {
        List<ACTION_TYPE> actions = new List<ACTION_TYPE>();
        while (playerActions.Count > 0)
        {
            if (playerActions[0].time <= timeElapsed)
            {
                actions.Add(playerActions[0].type);
                playerActions.Remove(playerActions[0]);
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
}
