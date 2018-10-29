using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier03Controller : Soldier {

    protected ACTION action;

    protected enum ACTION
    {
        ATTACK_BASE,
        //ATTACK_ENEMY,
        GOTO_BASE,
        //GOTO_ENEMY,

        NONE
    }

    
    Soldier targetSoldier;
    BaseBaseClass targetBase;

    public GameObject ShotPrefab;

    float attackTimer;
    bool attackedAlready;

	DamageBubbleController DamageBubbleC;
	HPBar MyHPBar;

	public void Init(UserData _owner)
    {
		MyHPBar = GetComponentInChildren<HPBar>(true);
		MyHPBar.gameObject.SetActive(false);//hide
		DamageBubbleC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();

		owner = _owner;
        fallen = owner.fallen;

        target = null;
        targetSoldier = null;
        targetBase = null;

        HP = ConfigController.Config.Soldier03MaxHP;
        action = ACTION.NONE;

        this.GetComponentInChildren<SpriteRenderer>().sprite = (!fallen ? angelSprite : devilSprite);

        attackTimer = 0;
        attackedAlready = false;
    }

    void Update () {
        if (getHP() <= 0)
            return;

        if (attackTimer >= ConfigController.Config.Soldier03AttackSpeed)
        {
            attackTimer = 0;
            attackedAlready = false;
        }
        attackTimer += Time.deltaTime;

        switch (action)
        {
            case ACTION.ATTACK_BASE:
                AttackBase();
                break;
            case ACTION.GOTO_BASE:
                if (targetBase.getHP() <= 0 || !target)
                {
                    action = ACTION.NONE;
                    return;
                }
                if (GetDistance(target) <= ConfigController.Config.Soldier03ShotRange)
                    action = ACTION.ATTACK_BASE;
                else
                    this.transform.Translate((target.position - this.transform.position).normalized * ConfigController.Config.Soldier03MoveSpeed * Time.deltaTime);
                break;
            case ACTION.NONE:
            default:
                Think();
                break;
        }
	}

    void Think()
    {
        action = ACTION.NONE;

        if(ACTION.NONE != action)
            return;//found an enemy

        //enemy attacked or no enemy around
        {
            //search for closest building
            targetBase = SearchForClosestEnemyBase();
            if (targetBase)
            {
                target = targetBase.transform;
                if (GetDistance(targetBase.transform) <= ConfigController.Config.Soldier03ShotRange)
                {
                    action = ACTION.ATTACK_BASE;
                    AttackBase();
                    return;
                }
                else
                {
                    action = ACTION.GOTO_BASE;
                    Vector3 diff = target.transform.position - transform.position;
                    diff.Normalize();
                    float rot = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
                    this.GetComponentInChildren<SpriteRenderer>().gameObject.transform.rotation = Quaternion.Euler(0, 0, -rot);
                    return;
                }
            }
            else //you won. no buildings.
                target = null;

        }
    }

    float GetDistance(Transform o)
    {
        return (this.transform.position - o.position).magnitude;
    }
  
    void AttackBase()
    {
        if (action != ACTION.ATTACK_BASE)
            return;

        if (!targetBase)
        {
            target = null;
            action = ACTION.NONE;//will change in next update()
            return;
        }
        if (targetBase.getHP() <= 0)
            action = ACTION.NONE;
        if (!targetBase.GetComponent<FarmBase>() &&
            !targetBase.GetComponent<BarrackBase01>() &&
            !targetBase.GetComponent<BarrackBase03>())
        {
            target = null;
            action = ACTION.NONE;//will change in next update()
            return;
        }

        if (attackedAlready) return;

        attackedAlready = true;
        //targetBase.Hurt(CalcDamage());
        GameObject missle = Instantiate(ShotPrefab, this.transform.position, 
                    Quaternion.identity, GameObject.FindGameObjectWithTag("OTHER").transform);
        missle.GetComponent<shotAI>().Init(owner, target.gameObject);

        
    }

    int CalcDamage()
    {
        int dmgVar = ConfigController.Config.Soldier03DmgVar;

        return ConfigController.Config.Soldier03Dmg + UnityEngine.Random.Range(0, 2 * dmgVar + 1) - dmgVar;
    }

    override public void Hurt(Soldier attacker, int damage)
    {
        HP -= damage;

		MyHPBar.SetHP(HP, ConfigController.Config.Soldier03MaxHP);
		DamageBubbleC.CreateDamageBubble(this.transform.position, damage);
        SFXController.PlaySound(SOUNDS.HIT);

        if (HP <= 0)
        {
            HP = 0;
            this.GetComponentInChildren<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f, 0.7f);//temp
            SFXController.PlaySound( !fallen ? SOUNDS.DIE_ANGEL : SOUNDS.DIE_DEVIL);
            gameController.soldiers.Remove(this);
            Destroy(this.gameObject);
            return;
        }

        targetSoldier = attacker;
        target = targetSoldier.transform;
    }


    BaseBaseClass SearchForClosestEnemyBase()
    {
        Transform closest = null;
        float close = Mathf.Infinity;
        List<FarmBase> A = (owner.amIPlayer ? gameController.AIData.Farms : gameController.playerData.Farms);
        List<BarrackBase01> B01 = (owner.amIPlayer ? gameController.AIData.Barracks01 : gameController.playerData.Barracks01);
        List<BarrackBase03> B03 = (owner.amIPlayer ? gameController.AIData.Barracks03 : gameController.playerData.Barracks03);

        foreach (var x in A)
        {
            if (x.getHP() <= 0) continue;
            if (GetDistance(x.transform) < close)
            {
                closest = x.transform;
                close = GetDistance(x.transform);
            }
        }
        foreach (var x in B01)
        {
            if (x.getHP() <= 0) continue;
            if (GetDistance(x.transform) < close)
            {
                closest = x.transform;
                close = GetDistance(x.transform);
            }
        }
        foreach (var x in B03)
        {
            if (x.getHP() <= 0) continue;
            if (GetDistance(x.transform) < close)
            {
                closest = x.transform;
                close = GetDistance(x.transform);
            }
        }
        if (!closest)
            return null;

        return closest.GetComponent<BaseBaseClass>();
    }

}
