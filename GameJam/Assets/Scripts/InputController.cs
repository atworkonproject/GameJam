using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputController : MonoBehaviour {

	public SelectedTileController SelectedTileC;
	void Start () {
		SelectedTileC = GameObject.FindWithTag("_SCRIPTS_").GetComponentInChildren<SelectedTileController>();
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())//left mouse pressed
		{
			SelectedTileC.Input_MouseLeftUp();
		}
	}

    private static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
