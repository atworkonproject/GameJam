using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier01Controller : Soldier {

    protected ACTION action;

    protected enum ACTION
    {
        ATTACK_BASE,
        ATTACK_ENEMY,
        GOTO_BASE,
        GOTO_ENEMY,

        NONE
    }

    Soldier targetSoldier;
    BaseBaseClass targetBase;

    float attackTimer;
    bool attackedAlready;

    public void Init(UserData _owner)
    {
        owner = _owner;
        fallen = owner.fallen;

        target = null;
        targetSoldier = null;
        targetBase = null;

        HP = ConfigController.Config.Soldier01MaxHP;
        action = ACTION.NONE;

        this.GetComponentInChildren<SpriteRenderer>().sprite = (!fallen ? angelSprite : devilSprite);

        attackTimer = 0;
        attackedAlready = false;
    }

    void Update () {
        if (getHP() <= 0)
            return;

        if (attackTimer >= ConfigController.Config.Soldier01AttackSpeed)
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
            case ACTION.ATTACK_ENEMY:
                AttackSoldier();
                break;
            case ACTION.GOTO_BASE:
                //if yo'll find closer enemy, then take it
                SearchForEnemiesAround();
                if (targetBase.getHP() <= 0)
                {
                    action = ACTION.NONE;
                    return;
                }
                if (GetDistance(target) <= attackDist)
                    action = ACTION.ATTACK_BASE;
                else
                    this.transform.Translate((target.position - this.transform.position).normalized * speed * Time.deltaTime);
                break;
            case ACTION.GOTO_ENEMY:
                //if yo'll find closer enemy, then take it
                SearchForEnemiesAround();
                if (targetSoldier.getHP() <= 0)
                {
                    action = ACTION.NONE;
                    return;
                }
                if (GetDistance(target) <= attackDist)
                    action = ACTION.ATTACK_ENEMY;
                else
                    this.transform.Translate((target.position - this.transform.position).normalized * speed * Time.deltaTime);
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

        //check if there is any enemy in range
        SearchForEnemiesAround();

        if(ACTION.NONE != action)
            return;//found an enemy

        //enemy attacked or no enemy around
        {
            //search for closest building
            targetBase = SearchForClosestEnemyBase();
            if(GetDistance(targetBase.transform) <= attackDist)
            {
                action = ACTION.ATTACK_BASE;
                target = targetBase.transform;
                AttackBase();
                return;
            }
            action = ACTION.GOTO_BASE;
            target = targetBase.transform;
        }
    }

    void SearchForEnemiesAround()
    {
        Soldier closest = null;
        float close = Mathf.Infinity;

        foreach (Soldier s in gameController.soldiers)
        {
            //if it's the same team - skip
            if (s.owner.fallen == owner.fallen)
                continue;

            if (s.getHP() <= 0)
                continue;

            if (GetDistance(s.transform) < close)
            {
                closest = s;
                close = GetDistance(s.transform);
            }
        }
        if(closest == null)//no enemies
        {
            return;
        }


        if (GetDistance(closest.transform) <= attackDist)
        {//is in attack distance - ATTACK
            targetSoldier = closest;
            target = targetSoldier.transform;
            action = ACTION.ATTACK_ENEMY;
            AttackSoldier();
            return;
        }
        if (GetDistance(closest.transform) <= rangeDist)
        {//is in range distance - GOTO ENEMY
            targetSoldier = closest;
            target = targetSoldier.transform;
            action = ACTION.GOTO_ENEMY;
            return;
        }
        return;//too far from it
    }

    float GetDistance(Transform o)
    {
        return (this.transform.position - o.position).magnitude;
    }

    void AttackSoldier()
    {
        if (attackedAlready) return;

        if (action != ACTION.ATTACK_ENEMY)
            return;

        if(!targetSoldier || !targetSoldier.GetComponent<Soldier01Controller>())
        {
            target = null;
            action = ACTION.NONE;//will change in next update()
            return;
        }

        attackedAlready = true;
        targetSoldier.Hurt(this, ConfigController.Config.Soldier01Dmg);
        if (targetSoldier.getHP() <= 0)
            action = ACTION.NONE;
    }
    void AttackBase()
    {
        if (attackedAlready) return;

        if (action != ACTION.ATTACK_BASE)
            return;

        if (!targetBase)
        {
            target = null;
            action = ACTION.NONE;//will change in next update()
            return;
        }
        if(!targetBase.GetComponent<FarmBase>() &&
            !targetBase.GetComponent<BarrackBase>())
        {
            target = null;
            action = ACTION.NONE;//will change in next update()
            return;
        }

        attackedAlready = true;
        targetBase.Hurt(ConfigController.Config.Soldier01Dmg);
        if (targetBase.getHP() <= 0)
            action = ACTION.NONE;
    }

    override public void Hurt(Soldier attacker, int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            this.GetComponentInChildren<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f, 0.7f);//temp
            gameController.soldiers.Remove(this);
            Destroy(this.gameObject);
            return;
        }

        targetSoldier = attacker;
        target = targetSoldier.transform;
        action = ACTION.ATTACK_ENEMY;
        AttackSoldier();
    }


    BaseBaseClass SearchForClosestEnemyBase()
    {
        Transform closest = null;
        float close = Mathf.Infinity;
        List<FarmBase> A = (owner.amIPlayer ? gameController.AIData.Farms : gameController.playerData.Farms);
        List<BarrackBase> B = (owner.amIPlayer ? gameController.AIData.Barracks : gameController.playerData.Barracks);

        foreach (var x in A)
        {
            if (x.getHP() <= 0) continue;
            if (GetDistance(x.transform) < close)
            {
                closest = x.transform;
                close = GetDistance(x.transform);
            }
        }
        foreach (var x in B)
        {
            if (x.getHP() <= 0) continue;
            if (GetDistance(x.transform) < close)
            {
                closest = x.transform;
                close = GetDistance(x.transform);
            }
        }
        return closest.GetComponent<BaseBaseClass>();
    }

}
