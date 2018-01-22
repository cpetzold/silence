using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	public int playerIndex = 0;
	public float walkSpeed = 8;
	public float runSpeed = 15;

	public float volumeMultiplier = 1;

	// DEBUG
	public Transform noiseCircle;

	public Player player;
	Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		SetPlayerIndex(playerIndex);
	}

	public void SetPlayerIndex(int index) {
		playerIndex = index;
		player = ReInput.players.GetPlayer(playerIndex);
	}
	
	void FixedUpdate () {
		float horizontal = player.GetAxis("Horizontal");
		float vertical = player.GetAxis("Vertical");
		bool running = player.GetButton("Run");

		Vector2 dir = new Vector2(horizontal, vertical);
		float speed = running ? runSpeed : walkSpeed;
		float noiseLevel = rb.velocity.magnitude * volumeMultiplier;
	
		rb.AddForce(dir * speed);

		if (noiseLevel > 0) {
			NoiseManager.MakeNoise(transform.position, noiseLevel);	
		}

		// DEBUG
		noiseCircle.localScale = Vector2.one * noiseLevel * 2;		
	}

	void Update(){
		if (player.GetButton("YELL")) {
			//NoiseManager.MakeNoise(transform.position, 30);
			//noiseCircle.localScale = Vector2.one * 60;
		}
	}
}
