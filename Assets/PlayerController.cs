using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	public int playerIndex = 0;
	public float speed = 5;

	Player player;
	Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		player = ReInput.players.GetPlayer(playerIndex);
	}
	
	void FixedUpdate () {
		float horizontal = player.GetAxis("Horizontal");
		float vertical = player.GetAxis("Vertical");
		Vector2 dir = new Vector2(horizontal, vertical);

		rb.AddForce(dir * speed);
	}
}
