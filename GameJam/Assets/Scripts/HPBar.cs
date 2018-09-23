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
		if (!this.gameObject.activeSelf)
			this.gameObject.SetActive(true);//show if damaged

		if (newHPValue <= 0)
		{
			newHPValue = 0.0f;
			this.gameObject.SetActive(false);
		}

		Foreground.transform.localScale = new Vector3(
			newHPValue / maxHPValue,
			Foreground.transform.localScale.y,
			Foreground.transform.localScale.z
			);
	}
}
