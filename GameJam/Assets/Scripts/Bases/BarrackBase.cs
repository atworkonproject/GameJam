using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackBase : BaseBaseClass {
    float timeToSpawn;
    public static float spawnEverySec = 5.0f;
    public static int COST_FOR_SOLDIER = 5;

    Bounds bounds;

    public GameObject Soldier01prefab;

	// Use this for initialization
	void Awake () {
        timeToSpawn = 0f;
        bounds = this.GetComponent<SpriteRenderer>().bounds;
    }
	
	// Update is called once per frame
	void Update () {
        timeToSpawn += Time.deltaTime;
        if (timeToSpawn >= spawnEverySec && UIController.uzywac_Credits >= COST_FOR_SOLDIER)
        {
            timeToSpawn = 0;
            GameObject soldier = Instantiate(Soldier01prefab, this.transform.position + new Vector3(0, bounds.size.y, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("SOLDIERS").transform);
            UIController.uzywac_Credits -= COST_FOR_SOLDIER;
        }
    }
}
