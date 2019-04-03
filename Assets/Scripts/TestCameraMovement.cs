using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMovement : MonoBehaviour {
    private Rigidbody rb;
    private float speed = 2;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

    }
}
