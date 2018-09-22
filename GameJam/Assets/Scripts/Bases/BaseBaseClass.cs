using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBaseClass : MonoBehaviour {
	//my indexes
	public Vector2Int MyIndexes;
    protected bool fallen;
    bool ownerIsPlayer;

    public void Init(bool isFallen, bool isPlayers)
    {
        fallen = isFallen;
        ownerIsPlayer = isPlayers;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setFallen(bool fall)
    {
        fallen = fall;
    }
}
