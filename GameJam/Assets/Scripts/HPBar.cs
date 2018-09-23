using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {
	public SpriteRenderer Foreground;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHP(float newHPValue, float maxHPValue)
	{
		Foreground.transform.localScale = new Vector3(
			Foreground.transform.localScale.x,
			newHPValue / maxHPValue,
			Foreground.transform.localScale.z
			);
	}
}
