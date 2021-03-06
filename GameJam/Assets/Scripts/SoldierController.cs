﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public SpriteRenderer sprRender;

    public LayerMask soldierLayer;

    public bool Fallen;

    public float SightRange;
    public float MoveSpeed;
    private float AttackRange = 0.5f;

    private float minimumDistance;
    private bool collided;

    public Sprite AngelSprite;
    public Sprite DevilSprite;

    CircleCollider2D Sight;

    public Vector3 targetPosition;
	private SoldierController _targetObjectSoldierController;
    private GameObject targetObject;

	public Vector2 MyOffsetToLeader;//if we go to leader we set this random offset

	public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public Collider2D[] Soldiers;

    public Strategies Strategy;

    [Header("Battle Stats")]

    public int MaxHp;
    public int Atk;
    public float AttackDelay;
    public int Charisma;

    public int Hp;
    private float lastAttack;
    private int leaderCharisma = 7;

    public enum Strategies
    {
        attackEnemy, attackBase, defend
    };

    // strategy Enemy attack




    // strategy Base attack

    public Collider2D[] Bases;

    public List<GameObject> FriendBases;
    public List<GameObject> EnemyBases;

    public LayerMask basesLayer;




    // strategy Defend

    Vector3 defendedPlace;



    // Use this for initialization
    void Start()
    {
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;
        targetPosition = transform.position;

        minimumDistance = 0.1f;

        AttackDelay = (float) Random.Range(2.0f, 4.0f);

        lastAttack = Time.time;
        Hp = MaxHp;

        switch (Strategy)
        {
            case Strategies.attackEnemy:

                break;

            case Strategies.attackBase:

                break;

            case Strategies.defend:
                defendedPlace = transform.position + new Vector3((float)Random.Range(AttackRange, SightRange), (float)Random.Range(0, SightRange), 0f);

                break;
        }



        /*random offset to leader but it shouldn't be zero
		float offsetX, offsetY;
		if (Random.Range(0, 1) == 0)
			offsetX = Random.Range(-1.5f, -.5f);
		else
			offsetX = Random.Range(.5f, 1.5f);
		if (Random.Range(0, 1) == 0)
			offsetY = Random.Range(-1.5f, -.5f);
		else
			offsetY = Random.Range(.5f, 1.5f);
		MyOffsetToLeader = new Vector2(offsetX, offsetY);
        */
    }

    // Update is called once per frame
    void Update()
    {
        checkHp();
        checkNearSoldiers();
        checkNearBuildings();

        switch (Strategy)
        {
            case Strategies.attackEnemy:
                if (Enemies.Count > 0)
                {
                    setTargetEnemyByObject();
                }
                else
                {
                    if (isTargetAchieved())
                    {
                        setRandomTargetPosition();
                    }
                }
                break;

            case Strategies.attackBase:
                if (Enemies.Count > 0)
                {
                    if (EnemyBases.Count > 0)
                    {
                        Debug.Log("widze wrogów i baze");
                        if (Random.Range(0,2)==0)
                        // z dwojga złego woli zaatakować budynek
                        {
                            Debug.Log("set target baze");
                            setTargetBaseByObject();
                        }
                        else
                        // z dwojga złego woli zaatakować wroga
                        {
                            Debug.Log("set target enemy");
                            setTargetEnemyByObject();
                        }
                    }
                    else
                    {
                        Debug.Log("widze tylko wrogów");
                        setTargetEnemyByObject();
                    }
                }
                else
                {
                    if (EnemyBases.Count > 0)
                    {
                        Debug.Log("widze tylko baze");
                        setTargetBaseByObject();
                    }
                    else if (isTargetAchieved())
                    {
                        setRandomTargetPosition();
                    }
                }
                break;

            case Strategies.defend:
                if (Enemies.Count > 0)
                {
                    setTargetEnemyByObject();
                }
                else
                {
                    targetPosition = defendedPlace;
                }
                break;
        }

    }


    private void FixedUpdate()
    {
        switch (Strategy)
        {
            case Strategies.attackEnemy:
                if (Enemies.Count > 0)
                {
                    if (isTargetAchieved())
                    {
                        doAction();
                    }
                    else
                    {
                        moveToTarget(1);
                    }
                }
                else
                {
                    if (!isTargetAchieved())
                    {
                        moveToTarget(1);
                    }
                }
                break;

            case Strategies.attackBase:
                if (Enemies.Count > 0 || EnemyBases.Count > 0)
                {
                    if (isTargetAchieved())
                    {
                        Debug.Log("atakuje baze lub wroga");
                        doAction();
                    }
                    else
                    {
                        Debug.Log("nie osiągnołem celu idę tam");
                        moveToTarget(1);
                    }
                }
                else
                {
                    Debug.Log("Idę do bazy");
                    if (!isTargetAchieved())
                    {
                        moveToTarget(1);
                    }

                }
                break;

            case Strategies.defend:
                if(Enemies.Count > 0)
                {
                    moveToTarget(1);
                }
                else
                {
                    if(!isTargetAchieved())
                    {
                        moveToTarget(1);
                    }
                }
                break;
        }

    }


    /*
    private void FixedUpdate()
    {
        float dist = (transform.position - targetPosition).magnitude;

        if (_targetObjectSoldierController != null)
            if (_targetObjectSoldierController.Fallen != Fallen)
            {
                //enemy
                if (dist > AttackRange)
                {
                    //Debug.Log("Poza zasiegiem ataku");
                    moveToTarget(1);
                }
                else if (dist < minimumDistance && targetObject != null)
                {
                    //Debug.Log("Za blisko: " + dist.ToString() + " < " + minimumDistance);
                    moveToTarget(-1);
                }
                else
                {
                    doAction();
                }
            }
        /*
        else
        {
            //leader
            if (dist > AttackRange)
            {
                //Debug.Log("Poza zasiegiem ataku");
                moveToLeaderTarget(1);
            }
            else if (dist < minimumDistance && targetObject != null)
            {
                //Debug.Log("Za blisko: " + dist.ToString() + " < " + minimumDistance);
                moveToLeaderTarget(-1);
            }
            else
            {
                doAction();
            }
        }


        if ((int)(Time.time * 10)%5 == 0 && outOfBounds())
        {
            Debug.Log(Time.time);
            Damage(1);
        }
    }
    */

    void setTargetEnemyByObject()
    {
        if (targetObject == null)
        {
            targetObject = Enemies[Random.Range(0, Enemies.Count)];
        }
        else
        {
            targetPosition = targetObject.transform.position;
        }
    }

    void setTargetBaseByObject()
    {
        if (targetObject == null)
        {
            Debug.Log("enemy Bases od [" + Random.Range(0, EnemyBases.Count));
            
            targetObject = EnemyBases[Random.Range(0, EnemyBases.Count)];

            Debug.Log(targetObject);
        }
        else
        {
            Debug.Log("mam już target obj");
            targetPosition = targetObject.transform.position;
        }
    }

    /*
    void setTarget()
    {
        
        bool tooClose = false;

        if (targetObject == null && !tooClose)
        {
            if (Enemies.Count > 0)
            {
                targetObject = Enemies[Random.Range(0, Enemies.Count)];
            }
            else if(Friends.Count > 0)
            {
                foreach(GameObject friend in Friends)
                {
                    SoldierController friendControl = friend.GetComponent<SoldierController>();
                    if(friendControl.Charisma > Charisma && friendControl.Charisma > leaderCharisma)
                    {
                        targetObject = friend;
                        break;
                    }
                }
            }
        }
        else if(!tooClose)
        {
            targetPosition = targetObject.transform.position;
        }
    }
    */

    void doAction()
    {
        if (targetObject != null)
        {
            if (targetObject.GetComponent<SoldierController>() != null)
            // Walka z przeciwnikiem
            {
                Debug.Log("Walka z przeciwnikiem");
                doAttack(targetObject);
            }
            else if (targetObject.GetComponent<BaseBaseClass>() != null)
            // Atak na budynek
            {
                Debug.Log("Walka z budynkiem");
                doDestroy(targetObject);
            }
        }
    }

	void moveToTarget(int turn)
	{
		Vector3 direction = Vector3.Normalize(targetPosition - transform.position) * MoveSpeed * Time.deltaTime;
		//Debug.Log("Chce się ruszyć do: " + turn.ToString() + " o wektor "+direction.ToString());

		transform.position += turn * direction;

	}

	void moveToLeaderTarget(int turn)
	{
		Vector3 direction = Vector3.Normalize((targetPosition + (Vector3)MyOffsetToLeader) - transform.position) * MoveSpeed * Time.deltaTime;
		//Debug.Log("Chce się ruszyć do: " + turn.ToString() + " o wektor "+direction.ToString());

		transform.position += turn * direction;

	}

	void setRandomTargetPosition()
    {
        float radius = Random.Range(0.0f, SightRange);
        float angle = Random.Range(Mathf.PI / 2 - 0.5f, Mathf.PI / 2 + 0.5f) * (gameController.playerData.fallen^Fallen ? -1 : 1);

        targetPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0.0f);
    }

    bool checkNearSoldiers()
    {
        Friends.Clear();
        Enemies.Clear();

        Soldiers = Physics2D.OverlapCircleAll(transform.position, SightRange, soldierLayer);

        foreach (Collider2D item in Soldiers)
        {
            GameObject obj = item.gameObject;
            SoldierController soldier = obj.GetComponent<SoldierController>();
            if (soldier.Fallen == Fallen)
            {
                if(!obj.Equals(gameObject))
                    Friends.Add(obj);
            }
            else
            {
                Enemies.Add(obj);
            }
        }
        return (Soldiers.Length > 0);
    }

    bool checkNearBuildings()
    {
        FriendBases.Clear();
        EnemyBases.Clear();

        //Debug.Log("Szukam baz");
        Bases = Physics2D.OverlapCircleAll(transform.position, SightRange, basesLayer);


        foreach (Collider2D item in Bases)
        {
            GameObject obj = item.gameObject;

            BaseBaseClass building = obj.GetComponent<BaseBaseClass>();

            if (building.owner.fallen == Fallen)
            {
                FriendBases.Add(obj);
            }
            else
            {
                EnemyBases.Add(obj);
            }
        }
        return (Bases.Length > 0);
    }


    
    bool isTargetAchieved()
    {
        float dist = (transform.position - targetPosition).magnitude;

        if (targetObject == null)
        {
            return dist < AttackRange;
        }
        else
        {

            bool near = dist < AttackRange;
            bool distant = dist > minimumDistance;

            //if(!near) Debug.Log("Za blisko: " + dist.ToString() + " < " + minimumDistance);
            //if (!distant) Debug.Log("Poza zasiegiem ataku");


            bool result = near && distant;

            return result;
        }
    }


    void doAttack(GameObject enemy)
    {
        SoldierController soldier = enemy.GetComponent<SoldierController>();

        if (Time.time - lastAttack > AttackDelay)
        {
            soldier.Damage(this);
            lastAttack = Time.time;
        }
    }

    void doDestroy(GameObject enemy)
    {
        BaseBaseClass building = enemy.GetComponent<BaseBaseClass>();

        if (Time.time - lastAttack > AttackDelay)
        {
            building.Damage(this);
            lastAttack = Time.time;
        }
    }

    public void Damage(SoldierController attacker)
    {
        int damage = Random.Range(0, attacker.Atk) + 1;
        Hp -= damage;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
		damBubbleController.CreateDamageBubble(transform.position, damage);
    }

    public void Damage(int damage)
    {
        Hp -= damage;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
        damBubbleController.CreateDamageBubble(transform.position, damage);
    }

    void checkHp()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool outOfBounds()
    {
        GameObject bg = GameObject.FindWithTag("BackgroundSprite");
        Sprite bgSpr = bg.GetComponent<SpriteRenderer>().sprite;

        Debug.Log(transform.position.y + " < " + (bg.transform.position.y - bgSpr.rect.height / 200).ToString());


        return (transform.position.x < bg.transform.position.x - bgSpr.rect.width / 200 ||
            transform.position.x > bg.transform.position.x + bgSpr.rect.width / 200 ||
            transform.position.y < bg.transform.position.y - bgSpr.rect.height / 200 ||
            transform.position.y > bg.transform.position.y + bgSpr.rect.height / 200);
    }


}
