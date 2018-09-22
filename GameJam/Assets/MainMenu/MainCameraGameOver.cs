using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCameraGameOver : MonoBehaviour {
	public float Speed = 0.5f;

	void Update()
	{

		transform.position = new Vector3(transform.position.x,
			Mathf.Lerp(transform.position.y, -2.0f, Time.deltaTime * Speed),//going down to y=-2.0f;
			transform.position.z);
	}
	public void OnRestartButtonClicked()
	{
		SceneManager.LoadScene(0);
	}

}
