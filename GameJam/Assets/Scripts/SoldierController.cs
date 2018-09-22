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
    public float AttackRange = 0.5f;

    private float minimumDistance = 0.2f;
    private bool collided;

    public Sprite AngelSprite;
    public Sprite DevilSprite;

    CircleCollider2D Sight;

    public Vector3 targetPosition;
    private GameObject targetObject;

    public List<GameObject> Friends;
    public List<GameObject> Enemies;
    public Collider2D[] Soldiers;


    [Header("Battle Stats")]

    public int MaxHp;
    public int Atk;
    public int AttackDelay;
    public int Charisma;

    private int Hp;
    private float lastAttack;


    // Use this for initialization
    void Start()
    {
        targetObject = null;
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;
        targetPosition = transform.position;


        lastAttack = Time.time;
        Hp = MaxHp;
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
        if (isTargetAchieved())
        {
            doAction();
        }
        else
        {
            moveToTarget(1);
        }
    }

    void setTarget()
    {
        if (targetObject == null)
        {
            if (Enemies.Count > 0)
            {
                targetObject = Enemies[Random.Range(0, Enemies.Count)];
            }
        }
        else
        {
            targetPosition = targetObject.transform.position;
        }
    }

    void doAction()
    {
        if (targetObject != null)
        // Walka z przeciwnikiem
        {
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
        if (targetObject == null)
        {
            Debug.Log("Jestem u celu");
            return (transform.position - targetPosition).magnitude < AttackRange;
        }
        else
        {
            Debug.Log("Idę na wroga. Odl = "+ (transform.position - targetPosition).magnitude);

            bool result = ((transform.position - targetPosition).magnitude < AttackRange) && ((transform.position - targetPosition).magnitude > minimumDistance);
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
    }

    void checkHp()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }



}
