using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopRedSprite : MonoBehaviour {
	public SpriteRenderer MySpriteRenderer;
	public float TimeLastDisplayed;
	// Use this for initialization
	void Start () {
		MySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		MySpriteRenderer.gameObject.SetActive(false);
	}
	public void Update()
	{
		if (Time.timeSinceLevelLoad - TimeLastDisplayed > 3.0f && MySpriteRenderer.gameObject.activeSelf)
			MySpriteRenderer.gameObject.SetActive(false);
	}
	

	public void ShowRedHalf()
	{
		MySpriteRenderer.gameObject.SetActive(true);
		TimeLastDisplayed = Time.timeSinceLevelLoad;
	}
}
