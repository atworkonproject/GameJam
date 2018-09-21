using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    public GameObject mapObject;
    private Camera cam;

    Vector2 targetPos;
    float targetSize;//zoom

	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();

        cam.orthographicSize = mapObject.GetComponent<MeshRenderer>().bounds.size.z * cam.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
