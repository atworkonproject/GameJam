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

    private List<GameObject> Friends;
    private List<GameObject> Enemies;

    private float positionResolution = 0.01f;

    //public Vector3 myPos;

    // Use this for initialization
    void Start()
    {
        Sight = GetComponent<CircleCollider2D>();
        sprRender.sprite = Fallen ? DevilSprite : AngelSprite;

        Sight.radius = SightRange;

        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( (transform.position - targetPosition).magnitude < positionResolution)
        {
            setNewTargetPosition();
        }
        else
        {
            moveToTarget();
        }
        //myPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoldierController other = collision.collider.gameObject.GetComponent<SoldierController>();
        if(other.Fallen != Fallen)
        // Spotkano Wroga
        {

        }
        else
        // Spotkano Sprzymierzeńca
        {

        }
    }

    void setNewTargetPosition()
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
