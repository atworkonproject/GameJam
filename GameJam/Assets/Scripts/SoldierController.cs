using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour {

    public SpriteRenderer sprRender;

    public bool Fallen;

    public float SightRange;
    public float MoveSpeed;
    public float AttackRange = 0.01f;

    public Sprite AngelSprite;
    public Sprite DevilSprite;

    CircleCollider2D Sight;

    public Vector3 targetPosition;
    private GameObject targetObject;

    public List<GameObject> Friends;
    public List<GameObject> Enemies;


    [Header("Battle Stats")]
    
    public int MaxHp;
    public int Atk;
    public int AttackDelay;

    private int Hp;
    private float lastAttack;

    // Use this for initialization
    void Start()
    {
        targetObject = null;
        Sight = GetComponent<CircleCollider2D>();
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;

        Sight.radius = SightRange;

        targetPosition = transform.position;


        lastAttack = Time.time;
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        checkHp();
        setTarget();

        if ((transform.position - targetPosition).magnitude < AttackRange)
        {
            doAction();
        }
        else
        {
            moveToTarget();
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
        if(targetObject != null)
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject otherObj = collider.transform.parent.gameObject;
        SoldierController other = otherObj.GetComponent<SoldierController>();

        if (!otherObj.Equals(gameObject))
        {
            if (other.Fallen != Fallen)
            // Spotkano Wroga
            {
                Enemies.Add(other.gameObject);
            }
            else
            // Spotkano Sprzymierzeńca
            {
                Friends.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject otherObj = collider.transform.parent.gameObject;
        SoldierController other = otherObj.GetComponent<SoldierController>();

        if (!otherObj.Equals(gameObject))
        {
            if (other.Fallen != Fallen)
            // Spotkano Wroga
            {
                Enemies.Remove(other.gameObject);
            }
            else
            // Spotkano Sprzymierzeńca
            {
                Friends.Remove(other.gameObject);
            }
        }
    }


    void moveToTarget()
    {
        Vector3 direction = Vector3.Normalize(targetPosition - transform.position);

        transform.position += direction * MoveSpeed;
    }

    void setRandomTargetPosition()
    { 
        float radius = Random.Range(0.0f,SightRange);
        float angle = Random.Range(Mathf.PI/2-0.5f,Mathf.PI/2+0.5f) * (Fallen?-1:1);

        targetPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0.0f);
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
