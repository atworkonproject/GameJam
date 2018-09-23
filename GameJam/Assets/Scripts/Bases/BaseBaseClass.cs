using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBaseClass : MonoBehaviour {
	//my indexes
	public Vector2Int MyIndexes;
    public UserData owner { get; protected set; }
    Vector2Int position;

    protected int HP = 1;

    public Sprite AngelBase;
    public Sprite DevilBase;

    public void Init(UserData _owner, Vector2Int pos)
    {
        owner = _owner;
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

    public void Damage(SoldierController attacker)
    {
        int damage = Random.Range(0, attacker.Atk) + 1;
        HP -= damage;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
        damBubbleController.CreateDamageBubble(transform.position, damage);

        if(HP <= 0)
        {
            HP = 0;
            //destroy this building - change the color
            this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void Damage(int atk)
    {
        HP -= atk;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
        damBubbleController.CreateDamageBubble(transform.position, atk);

        if (HP <= 0)
        {
            HP = 0;
            //destroy this building - change the color
            this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
