using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBases : MonoBehaviour {

	public static List<FarmBase> PlayerFarmsStatic;
	public List<FarmBase> PlayerFarms;//to view in inspector
	public static List<BarrackBase> PlayerBarracksStatic;
	public List<BarrackBase> PlayerBarracks;//to view in inspector

	void Start () {
		PlayerFarms = new List<FarmBase>();
		PlayerFarmsStatic = PlayerFarms;
		PlayerBarracks = new List<BarrackBase>();
		PlayerBarracksStatic = PlayerBarracks;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
