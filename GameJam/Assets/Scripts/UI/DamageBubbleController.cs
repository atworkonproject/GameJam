using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBubbleController : MonoBehaviour {
	[Header("ToLink")]
	public Canvas DamageBubbleCanvas;
	[Header("other")]
	public DamageBubble FirstDamageBubble;

	// Use this for initialization
	void Start () {
		FirstDamageBubble = DamageBubbleCanvas.GetComponentInChildren<DamageBubble>();
		FirstDamageBubble.gameObject.SetActive(false);//hide
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//uzycie
	/*
	 * 		if (Time.timeSinceLevelLoad % 1.0f == 0.0f)//every second
		{
			DamageBubbleController damBubbleController =//todo cache
					GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<DamageBubbleController>();
			damBubbleController.CreateDamageBubble(transform.position, Atk);
		}
		*/
	public void CreateDamageBubble(Vector2 worldPosition, float damagePositive, bool isPositive = false)
	{
		GameObject go = Instantiate(FirstDamageBubble.gameObject);
		DamageBubble createdDamageBubble = go.GetComponent<DamageBubble>();
		createdDamageBubble.name = "-" + damagePositive.ToString();
		if (isPositive)
		{
			createdDamageBubble.GetComponent<Text>().text = damagePositive.ToString();
			createdDamageBubble.GetComponent<Text>().color = Color.green;
		}
		else
		{
			createdDamageBubble.GetComponent<Text>().text = "-" + damagePositive;
			createdDamageBubble.GetComponent<Text>().color = Color.red;
		}
		createdDamageBubble.gameObject.SetActive(true);
		createdDamageBubble.transform.SetParent(DamageBubbleCanvas.transform);

		GameObject camGO = GameObject.FindWithTag("MainCamera");
		Vector3 screenPoint = camGO.GetComponent<Camera>().WorldToScreenPoint(worldPosition);
		createdDamageBubble.transform.position = screenPoint;
	}
}
