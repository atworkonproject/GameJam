using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    protected int HP;
    protected bool fallen; //is it fallen
    public UserData owner { get; protected set; }
    protected Transform target;

    public Sprite angelSprite, devilSprite;
    
    protected float attackDist = 0.16f, rangeDist = 2.0f;

    public int getHP() { return HP; }
	public void Start()
	{

	}
	public virtual void Hurt(Soldier attacker, int damage)
    {
        //HP -= damage;
        //if(HP <= 0)
        //{
        //    HP = 0;
        //    this.GetComponentInChildren<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);//temp
        //    gameController.soldiers.Remove(this);
        //    Destroy(this.gameObject);
        //    return;
        //}

        //attack the soldier now ... to do 
    }
}