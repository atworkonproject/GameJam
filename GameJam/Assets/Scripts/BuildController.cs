using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {
	[Header("to link")]
	public GameObject Barracks1BasePrefab;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildBarracks1(Vector2 position)
	{
		Instantiate(Barracks1BasePrefab).transform.position = position;
	}
}
