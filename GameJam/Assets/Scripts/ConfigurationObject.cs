using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to access, use static field ConfigController.Config

[CreateAssetMenu(menuName = "My Assets/ConfigurationObject")]
public class ConfigurationObject : ScriptableObject {
	public float startPlayerCredits = 100.0f;
	public float maxPlayerCredits = 200.0f;
    [Header("HP of buildings")]
    public int Barracks01MaxHP = 150;
    public int Barracks03MaxHP = 200;
    public int FarmMaxHP = 100;

    [Header("HP and DMG of soldiers")]
    public float minCollisionDist = 0.16f;
    public int Soldier01MaxHP = 75;
    public int Soldier01Dmg = 25;
    public int Soldier01DmgVar = 3;
	public float Soldier01AttackSpeed = 0.5f;//every n sec
	public float Soldier01MoveSpeed = 5.0f;//every n sec
    public float Soldier01SeeRange = 2.0f;

    public int Soldier03MaxHP = 150;
    public int Soldier03Dmg = 100;
    public int Soldier03DmgVar = 25;
    public float Soldier03AttackSpeed = 1.5f;//every n sec
    public float Soldier03MoveSpeed = 2.0f;//every n sec
    public float Soldier03ShotRange = 2.0f;
    public float Soldier03MissleSpeed = 4.0f;

    [Header("BaseBuy")]
	public float Barracks01BuyCost = 20.0f;
    public float Barracks02BuyCost = 30.0f;
    public float Barracks03BuyCost = 50.0f;
    public float FarmBuyCost = 20.0f;

    [Header("BaseOperate")]
	public float FarmEarn = 10.0f;
	public float FarmEarnPeriod = 5.0f;
	public float CostForSoldier01 = 5.0f;
    public float CostForSoldier03 = 10.0f;
    public float Barrack01SpawnEverySec = 5.0f;
    public float Barrack03SpawnEverySec = 10.0f;

    [Header("Other")]
	public float InfoTextDisplayTime = 3.0f;
    public float AIDecisionEverySec = 2.1f;
    public float BUILDING_TIME = 7.0f;
    public float MAX_TIME_FOR_AI_TO_START = 4.0f;//if the time tom first action is too long then cut it to <-
    //public float CameraMinZoom = 0.2f;
    //public float CameraZoomSpeed = 1.4f;
}

