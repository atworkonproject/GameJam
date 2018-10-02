using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBaseClass : MonoBehaviour {
	//my indexes
	public Vector2Int MyIndexes;
    public UserData owner { get; protected set; }
    Vector2Int position;
	public HPBar MyHPBar;

    protected int HP = 1;

    public Sprite AngelBase;
    public Sprite DevilBase;

    public Sprite AngelBaseShadow;
    public Sprite DevilBaseShadow;
    public SpriteRenderer shadow;

    public void Init(UserData _owner, Vector2Int pos)
    {
		MyHPBar = GetComponentInChildren<HPBar>().GetComponent<HPBar>();
		MyHPBar.gameObject.SetActive(false);//hide
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

    public int getHP() { return HP; }
    virtual protected int getMaxHP() { return 10; }
    public void Hurt(int atk)
    {
        if (HP <= 0) return;

        HP -= atk;
        if (HP <= 0) HP = 0;

		if (this is FarmBase)
		{
			MyHPBar.SetHP(HP, ConfigController.Config.FarmMaxHP);
		}
		else if (this is BarrackBase01)
		{
			MyHPBar.SetHP(HP, ConfigController.Config.BarracksMaxHP);
		}

		DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
        damBubbleController.CreateDamageBubble(transform.position, atk);

        //destroy this building - change the color
        float color = (float)HP / (float)getMaxHP();
        this.GetComponent<SpriteRenderer>().color = new Color(color, color, color);

        if (HP <= 0)
        {
            SFXController.PlaySound(SOUNDS.BASE_DESTROY);
            var bc = GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>();
            if (owner.amIPlayer)
                bc.playerAliveBases--;
            else
                bc.AIaliveBases--;
        }
    }
}
