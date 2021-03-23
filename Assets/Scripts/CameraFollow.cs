using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
	Camera mainCamera;
	
    void Start()
    {
		mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3 (transform.position.x + offset.x, transform.position.y + offset.y, offset.z);
    }
}
