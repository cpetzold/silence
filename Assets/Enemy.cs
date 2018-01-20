using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public Transform destination;
	public float speed = 1;
	public float nextWaypointDistance = 3;

	Vector2 currentDestination;

	Rigidbody2D rb;
	Path currentPath;
	int currentWaypoint;
	Seeker seeker;

	// Use this for initialization
	void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		StartCoroutine(RecalcPath());
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (currentPath == null) return;

		if (currentWaypoint > currentPath.vectorPath.Count) return;
        if (currentWaypoint == currentPath.vectorPath.Count) {
            Debug.Log("End Of Path Reached");
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
		currentDestination = destination.position;
	}

	IEnumerator RecalcPath(){
		while(true){
			if(currentDestination.x != destination.position.x || currentDestination.y != destination.position.y){
				seeker.StartPath(transform.position, destination.position, OnPathComplete);
				currentDestination = destination.position;
			}
			yield return new WaitForSeconds(.01f);
		}
		
	}
}
