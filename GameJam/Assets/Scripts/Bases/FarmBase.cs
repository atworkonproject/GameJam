using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : BaseBaseClass {
	public float LastTimeFarmEarned;

	// Update is called once per frame
	void Update () {

	}

    override public void Init2()
    {
        GetComponent<SpriteRenderer>().sprite = (owner.fallen) ? DevilBase : AngelBase;
        HP = getMaxHP();
    }

    override protected int getMaxHP() { return ConfigController.Config.FarmMaxHP; }
}
