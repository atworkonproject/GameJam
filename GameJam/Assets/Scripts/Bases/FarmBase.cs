using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : BaseBaseClass {
	public float LastTimeFarmEarned;
	// Use this for initialization
	void Start () {
		MaxHP = ConfigController.Config.FarmHP;
        HP = MaxHP;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
