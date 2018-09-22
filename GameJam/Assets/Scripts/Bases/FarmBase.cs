using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBase : BaseBaseClass {
	public float LastTimeFarmEarned;
	// Use this for initialization
	void Start () {
		HP = ConfigController.Config.FarmHP;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
