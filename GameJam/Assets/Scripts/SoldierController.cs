using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour {

    public SpriteRenderer sprRender;

    public bool Fallen;

    public float SightRange;

    public float MoveSpeed;

    public Sprite AngelSprite;
    public Sprite DevilSprite;

    CircleCollider2D Sight;

    public Vector3 targetPosition;
    private GameObject targetObject;

    public List<GameObject> Friends;
    public List<GameObject> Enemies;

    public float AttackRange = 0.01f;
    public int MaxHP;
    public int Attack;


    // Use this for initialization
    void Start()
    {
        targetObject = null;
        Sight = GetComponent<CircleCollider2D>();
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;

        Sight.radius = SightRange;

        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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


    void setRandomTargetPosition()
    { 
        float radius = Random.Range(0.0f,SightRange);
        float angle = Random.Range(0.0f, 2 * Mathf.PI);

        targetPosition = transform.position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0.0f);
    }




    void moveToTarget()
    {
        Vector3 direction = Vector3.Normalize(targetPosition - transform.position);

        transform.position += direction * MoveSpeed;
    }

}
