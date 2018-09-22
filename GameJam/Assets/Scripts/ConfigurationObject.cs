using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to access, use static field ConfigController.Config

[CreateAssetMenu(menuName = "My Assets/ConfigurationObject")]
public class ConfigurationObject : ScriptableObject {
	public float startPlayerCredits = 100.0f;
	public float maxPlayerCredits = 200.0f;
	[Header("BaseBuy")]
	public float BarracksBuyCost = 20.0f;
	public float FarmBuyCost = 20.0f;
	[Header("BaseOperate")]
	public float FarmEarnPerSecond = 10.0f;
	public float CostForSoldier = 5.0f;
	public float BarrackSpawnEverySec = 5.0f;
	[Header("Other")]
	public float InfoTextDisplayTime = 3.0f;
    public float AIDecisionEverySec = 2.1f;
}

