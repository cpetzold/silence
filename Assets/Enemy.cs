using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public float speed = 1;
	public float nextWaypointDistance = 3;
	public float decayRate = 5;

	Vector2 currentDestination;
	public float currentNoiseOfTarget;

	Rigidbody2D rb;
	Path currentPath;
	int currentWaypoint;
	Seeker seeker;

	// Use this for initialization
	void Start() { 
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		NoiseManager.instance.OnNoiseMade.AddListener(HandleNoiseEvent);
	}
	
	void Destroy(){
		NoiseManager.instance.OnNoiseMade.RemoveListener(HandleNoiseEvent);
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		DetermineTarget();
		MoveAlongPath();

		currentNoiseOfTarget = Mathf.Max(0, currentNoiseOfTarget - decayRate * Time.fixedDeltaTime);
	}

	void DetermineTarget(){
		

		
	}

	void MoveAlongPath(){
		if (currentPath == null) return;

		if (currentWaypoint > currentPath.vectorPath.Count) return;
        if (currentWaypoint == currentPath.vectorPath.Count) {
            Debug.Log("End Of Path Reached");
			currentNoiseOfTarget = 0;
            currentWaypoint++;
            return;
        }

		Vector3 dir = (currentPath.vectorPath[currentWaypoint]-transform.position).normalized;
		rb.AddForce(dir * speed);

		if ((transform.position-currentPath.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
	}

	void OnPathComplete(Path p) {
		currentPath = p;
		currentWaypoint = 1;
	}

	void SetDestination(Vector2 newDest){
		if(currentDestination.x != newDest.x || currentDestination.y != newDest.y){
			seeker.StartPath(transform.position, newDest, OnPathComplete);
			currentDestination = newDest;
		}
	}

	public void HandleNoiseEvent(NoiseEventData data){
		float dist = Vector2.Distance(transform.position, data.position);

		float perceivedNoise = 1 - (dist / data.radius);
		
		if(perceivedNoise > currentNoiseOfTarget){
			SetDestination(data.position);
			currentNoiseOfTarget = perceivedNoise;
		}

	}


}
