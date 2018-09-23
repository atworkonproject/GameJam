using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject mapObject;
    private Bounds mapBounds;
    private Camera cam;

    float targetSize;//zoom

    private float maxOrthoSize = 1.0f;
    public float minZoom; //% of maxZoom
    public float scrollSensitivity;

    public float CAM_MARGIN = 1.0f;//margin outside of the map

    Vector3 mouseLastPos, moveVector;

	// Use this for initialization
	void Awake () {
        cam = this.GetComponent<Camera>();
        mapBounds = mapObject.GetComponent<SpriteRenderer>().bounds;
        mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveVector = Vector3.zero;
        minZoom = 0.2f;
        scrollSensitivity = 1.0f;

        SetZoomToWholeMap();
    }
	
	// Update is called once per frame
	void Update () {
        //zoom
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            targetSize += -Input.mouseScrollDelta.y * scrollSensitivity;
            if (targetSize < minZoom * maxOrthoSize)
                targetSize = minZoom * maxOrthoSize;
            else if (targetSize > maxOrthoSize)
                targetSize = maxOrthoSize;
        }

        float newCamSize = Mathf.Lerp(cam.orthographicSize, targetSize, 10f * Time.deltaTime);
        UpdateSize(newCamSize);

        //moving camera
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
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
        {
            newCamSize = maxOrthoSize;
            targetSize = 0;
        }
        else if (newCamSize < minZoom * maxOrthoSize)
        {
            newCamSize = minZoom * maxOrthoSize;
            targetSize = 0;
        }

        cam.orthographicSize = newCamSize;
    }

    void SetZoomToWholeMap()
    {
        float mapX = mapBounds.size.x;
        float mapY = mapBounds.size.y;
        if (mapX / mapY >= cam.aspect)//use height, map is wider than cam size
            cam.orthographicSize = mapBounds.size.y / 2.0f;
        else
            cam.orthographicSize = mapBounds.size.x / cam.aspect / 2.0f;

        maxOrthoSize = cam.orthographicSize;
        targetSize = maxOrthoSize;
        cam.transform.position = mapObject.transform.position;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
    }

    void UpdatePosition(Vector3 moveBy)//checks if out of bounds etc
    {
        //set correct position
        float cX = cam.orthographicSize * cam.aspect * 2;
        float cY = cam.orthographicSize * 2;
        Rect c = new Rect(cam.transform.position.x -cX/2.0f + moveBy.x, cam.transform.position.y -cY/2.0f + moveBy.y, cX, cY);
        Rect m = new Rect(mapObject.transform.position -mapBounds.size/2.0f + new Vector3(-CAM_MARGIN, -CAM_MARGIN, 0), mapBounds.size + new Vector3(2* CAM_MARGIN,2* CAM_MARGIN, 0));
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
