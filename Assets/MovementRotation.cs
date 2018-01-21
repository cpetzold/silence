using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotation : MonoBehaviour {

	Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
		if (rb.velocity.magnitude > 0) {
			transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
		}
	}
}
