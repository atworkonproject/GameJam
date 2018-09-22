using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public SpriteRenderer sprRender;

    public LayerMask soldierLayer;

    public bool Fallen;

    public float SightRange;
    public float MoveSpeed;
    private float AttackRange;

    private float minimumDistance;
    private bool collided;

    public Sprite AngelSprite;
    public Sprite DevilSprite;

    CircleCollider2D Sight;

    public Vector3 targetPosition;
	private SoldierController _targetObjectSoldierController;
	private GameObject targetObject
	{
		get {
			if (_targetObjectSoldierController == null)
				return null;
			else
				return _targetObjectSoldierController.gameObject;
		}
		set
		{
			if (value == null)
				_targetObjectSoldierController = null;
			else
				_targetObjectSoldierController = value.GetComponent<SoldierController>();
		}
	}
	public Vector2 MyOffsetToLeader;//if we go to leader we set this random offset

	public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public Collider2D[] Soldiers;

    [Header("Battle Stats")]

    public int MaxHp;
    public int Atk;
    public float AttackDelay;
    public int Charisma;

    private int Hp;
    private float lastAttack;
    private int leaderCharisma = 7;


    // Use this for initialization
    void Start()
    {
		targetObject = null;
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;
        targetPosition = transform.position;

        minimumDistance = 0.1f;

        AttackDelay = (float) Random.Range(2.0f, 4.0f);

        lastAttack = Time.time;
        Hp = MaxHp;
        Charisma = Random.Range(0, 10);

		//random offset to leader but it shouldn't be zero
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
    }

    // Update is called once per frame
    void Update()
    {
        checkNearSoldiers();
        checkHp();
        setTarget();
    }

    private void FixedUpdate()
    {
		float dist = (transform.position - targetPosition).magnitude;

		if (_targetObjectSoldierController != null)
			if(_targetObjectSoldierController.Fallen != gameController.playerData.fallen)
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

        
    }

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

    void doAction()
    {
        if (targetObject != null)
        // Walka z przeciwnikiem
        {
            if(targetObject.GetComponent<SoldierController>().Fallen != Fallen)
                doAttack(targetObject);
        }
        else
        // Ustaw nowy cel
        {
            setRandomTargetPosition();
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
        float angle = Random.Range(Mathf.PI / 2 - 0.5f, Mathf.PI / 2 + 0.5f) * (Fallen ? -1 : 1);

        targetPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0.0f);
    }

    void checkNearSoldiers()
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
            
            bool result = near && distant;

            return result;
        }
    }


    void doAttack(GameObject enemy)
    {
        SoldierController soldier = enemy.GetComponent<SoldierController>();

        if (Time.time - lastAttack > AttackDelay)
        {
            soldier.Damage(Random.Range(0, Atk) + 1);
            lastAttack = Time.time;
        }
    }

    public void Damage(int atk)
    {
        Hp -= atk;

        DamageBubbleController damBubbleController = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
		damBubbleController.CreateDamageBubble(transform.position, atk);
    }

    void checkHp()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }



}
