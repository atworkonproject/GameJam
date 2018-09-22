using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBubble : MonoBehaviour {
	private float TimeCreated;
	public TextMesh MyTextMesh;
	// Use this for initialization
	void Start () {
		TimeCreated = Time.timeSinceLevelLoad;
		MyTextMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		float bubbleSpeed = 1f;
		float timeToLive = 2.0f;

		transform.position = new Vector3(
			transform.position.x,
			transform.position.y + (bubbleSpeed * Time.deltaTime),
			transform.position.z
			);

		if (Time.timeSinceLevelLoad - TimeCreated > timeToLive)
			Destroy(this.gameObject);
	}
}
