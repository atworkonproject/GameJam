using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bases : MonoBehaviour {

	public static List<FarmBase> PlayerFarmsStatic;
	public List<FarmBase> PlayerFarms;//to view in inspector
	public static List<BarrackBase> PlayerBarracksStatic;
	public List<BarrackBase> PlayerBarracks;//to view in inspector

    public static List<FarmBase> OpponentFarmsStatic;
    public List<FarmBase> OpponentFarms;//to view in inspector
    public static List<BarrackBase> OpponentBarracksStatic;
    public List<BarrackBase> OpponentBarracks;//to view in inspector

    void Start () {
		PlayerFarms = new List<FarmBase>();
		PlayerFarmsStatic = PlayerFarms;
		PlayerBarracks = new List<BarrackBase>();
		PlayerBarracksStatic = PlayerBarracks;

        OpponentFarms = new List<FarmBase>();
        OpponentFarmsStatic = PlayerFarms;
        OpponentBarracks = new List<BarrackBase>();
        OpponentBarracksStatic = PlayerBarracks;
    }
	
    public static void DestroyAllBases()
    {
        foreach (var v in PlayerFarmsStatic)
            Destroy(v.gameObject);
        foreach (var v in PlayerBarracksStatic)
            Destroy(v.gameObject);
        PlayerFarmsStatic.Clear();
        PlayerBarracksStatic.Clear();

        foreach (var v in OpponentFarmsStatic)
            Destroy(v.gameObject);
        foreach (var v in OpponentBarracksStatic)
            Destroy(v.gameObject);
        OpponentFarmsStatic.Clear();
        OpponentBarracksStatic.Clear();
    }
}
