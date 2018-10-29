using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotAI : MonoBehaviour {

    GameObject target;
    UserData owner;

	// Use this for initialization
	void Awake () {
		
	}

    public void Init(UserData owner, GameObject target)
    {
        this.owner = owner;
        this.target = target;
    }

    // Update is called once per frame
    void Update () {
        if(!target)
        {
            Destroy(this.gameObject);
            return;
        }

        if (GetDistance(target.transform) <= ConfigController.Config.minCollisionDist)
        {
            BaseBaseClass enemy = target.gameObject.GetComponent<BaseBaseClass>();
            if(enemy)
                enemy.Hurt(CalcDamage());
            Destroy(this.gameObject);
            return;
        }
        this.transform.Translate((target.transform.position - this.transform.position).normalized * ConfigController.Config.Soldier03MissleSpeed * Time.deltaTime);
    }

    float GetDistance(Transform o)
    {
        return (this.transform.position - o.position).magnitude;
    }

    int CalcDamage()
    {
        int dmgVar = ConfigController.Config.Soldier03DmgVar;

        return ConfigController.Config.Soldier03Dmg + UnityEngine.Random.Range(0, 2 * dmgVar + 1) - dmgVar;
    }

    //private bool TakeDamage()
}
