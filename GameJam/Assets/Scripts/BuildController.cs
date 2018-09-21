using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	[Header("to link")]
	public GameObject Barracks1BasePrefab;
	public GameObject FarmBasePrefab;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildBarracks(Vector2 position)
	{
		Instantiate(Barracks1BasePrefab).transform.position = new Vector3(position.x, position.y, -1.0f);//-1.0f to be in front of backgroundSprite
	}
	public void BuildFarm(Vector2 position)
	{
		Instantiate(FarmBasePrefab).transform.position = new Vector3(position.x, position.y, -1.0f);//-1.0f to be in front of backgroundSprite
	}
}
