using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCameraMainMenu : MonoBehaviour {

	//public Camera MainCamera;
	public Transform ButonsPanel;
	public Transform CreditsPanel;
	public float Speed = 0.5f;
	// Use this for initialization
	void Start () {
        //MainCamera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x,
			Mathf.Lerp(transform.position.y, 1.0f, Time.deltaTime * Speed),//going up to y=0.0f;
			transform.position.z);
	}

	public void OnStartClicked()
	{
		SceneManager.LoadScene(1);
	}

	public void OnExitClicked()
	{
        Application.Quit();
	}

	public void OnCreditsClicked()
	{
		ButonsPanel.gameObject.SetActive(false);
		CreditsPanel.gameObject.SetActive(true);
	}

	public void OnCreditsBackButtonClicked()
	{
		ButonsPanel.gameObject.SetActive(true);
		CreditsPanel.gameObject.SetActive(false);
	}

}
