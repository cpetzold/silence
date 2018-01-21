using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	public int playerIndex = 0;
	public float speed = 5;
	public float volumeMultiplier = 1;
	public Transform noiseCircle;

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

		if (dir.magnitude > 0) {
			NoiseManager.MakeNoise(transform.position, dir.magnitude * volumeMultiplier);	
		}

		noiseCircle.localScale = Vector2.one * (dir.magnitude * volumeMultiplier * 2);

		rb.AddForce(dir * speed);
	}

	void Update(){
		if (player.GetButton("YELL")) {
			NoiseManager.MakeNoise(transform.position, 30);
			noiseCircle.localScale = Vector2.one * 60;
		}
	}
}
