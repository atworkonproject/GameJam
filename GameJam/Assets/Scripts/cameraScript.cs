using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject mapObject;
    private Camera cam;

    Vector2 targetPos;
    float targetSize = 1.0f;//zoom

    private float maxOrthoSize = 1.0f;
    public float minZoom = 0.2f;//% of maxZoom
    public float moveSensitivity = 0.1f;

    Vector3 mouseLastPos, moveVector;

	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
        targetPos = cam.transform.position;
        mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveVector = Vector3.zero;

        SetZoomToWholeMap();
    }
	
	// Update is called once per frame
	void Update () {
        //zoom
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            targetSize += Input.mouseScrollDelta.y;
            if (targetSize < minZoom * maxOrthoSize)
                targetSize = minZoom * maxOrthoSize;
            else if (targetSize > maxOrthoSize)
                targetSize = maxOrthoSize;
        }

        float newCamSize = Mathf.Lerp(cam.orthographicSize, targetSize, 10f * Time.deltaTime);
        UpdateSize(newCamSize);

        //moving camera
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
                mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            moveVector = mouseLastPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.Translate(moveVector);

        }
        else
            moveVector -= 4.0f * moveVector * Time.deltaTime;

        UpdatePosition(moveVector);

        mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void UpdateSize(float newCamSize)
    {
        //set correct size
        if (newCamSize > maxOrthoSize)
            newCamSize = maxOrthoSize;
        else if (newCamSize < minZoom * maxOrthoSize)
            newCamSize = minZoom * maxOrthoSize;

        cam.orthographicSize = newCamSize;
    }

    void SetZoomToWholeMap()
    {
        float mapX = mapObject.GetComponent<MeshRenderer>().bounds.size.x;
        float mapY = mapObject.GetComponent<MeshRenderer>().bounds.size.y;
        if (mapX / mapY >= cam.aspect)//use height, map is wider than cam size
            cam.orthographicSize = mapObject.GetComponent<MeshRenderer>().bounds.size.y / 2.0f;
        else
            cam.orthographicSize = mapObject.GetComponent<MeshRenderer>().bounds.size.x / cam.aspect / 2.0f;

        maxOrthoSize = cam.orthographicSize;
        cam.transform.position = mapObject.transform.position;
    }

    void UpdatePosition(Vector3 moveBy)//checks if out of bounds etc
    {
        //set correct position
        float cX = cam.orthographicSize * cam.aspect * 2;
        float cY = cam.orthographicSize * 2;
        Rect c = new Rect(cam.transform.position.x -cX/2.0f + moveBy.x, cam.transform.position.y -cY/2.0f + moveBy.y, cX, cY);
        Rect m = new Rect(mapObject.transform.position - mapObject.GetComponent<MeshRenderer>().bounds.size/2.0f, mapObject.GetComponent<MeshRenderer>().bounds.size);
        if (c.x < m.x)
        {
            moveBy.x += m.x - c.x;
            moveVector.x = 0;
        }
        if (c.y < m.y)
        {
            moveBy.y += m.y - c.y;
            moveVector.y = 0;
        }
        if (c.x + c.width > m.x + m.width)
        {
            moveBy.x += m.x + m.width - (c.x + c.width);
            moveVector.x = 0;
        }
        if (c.y + c.height > m.y + m.height)
        {
            moveBy.y += m.y + m.height - (c.y + c.height);
            moveVector.y = 0;
        }

        cam.transform.Translate(moveBy);
    }
}
