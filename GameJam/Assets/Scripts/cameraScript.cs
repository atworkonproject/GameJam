using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject mapObject;
    private Camera camera;

    float defaultSize;

    Vector2 targetPos;
    float targetSize;//zoom

	// Use this for initialization
	void Start () {
        camera = this.GetComponent<Camera>();

        camera.orthographicSize = defaultSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
