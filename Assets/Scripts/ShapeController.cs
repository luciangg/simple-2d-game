using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
	private Rigidbody2D rigidBody2D;
    void Start()
    {
        Destroy(gameObject, 5f);
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rigidBody2D.rotation += 10.0f;
    }	
	// void OnTriggerEnter2D(Collider2D col)
	// {
		// Destroy(gameObject);
	// }
}
