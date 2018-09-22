using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBaseClass : MonoBehaviour {
	//my indexes
	public Vector2Int MyIndexes;
    protected bool fallen;
    bool ownerIsPlayer;
    Vector2Int position;

    public int MaxHP;
	protected int HP;

    public void Init(bool isFallen, bool isPlayers, Vector2Int pos)
    {
        fallen = isFallen;
        ownerIsPlayer = isPlayers;
        position = pos;
        Init2();
    }

    public virtual void Init2()
    {
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Damage(int atk)
    {
        HP -= atk;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
        damBubbleController.CreateDamageBubble(transform.position, atk);
    }
}
