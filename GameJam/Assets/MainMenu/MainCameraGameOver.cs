using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCameraGameOver : MonoBehaviour {
	public float Speed = 0.5f;

	private void Start()
	{
		//destroy all gameBubbles
		DamageBubble[] damageBubbleArray = GameObject.FindObjectsOfType<DamageBubble>();
		foreach (var bubble in damageBubbleArray)
			Destroy(bubble.gameObject);
	}

	void Update()
	{

		transform.position = new Vector3(transform.position.x,
			Mathf.Lerp(transform.position.y, -2.5f, Time.deltaTime * 0.5f),//going down to y=-2.0f;
			transform.position.z);
	}
	public void OnRestartButtonClicked()
	{
        Initiate.Fade("MainMenu", new Color(0, 0, 0), 0.7f);
	}

}
