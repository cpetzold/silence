using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState {
    Patrolling, Chasing, Searching
}

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

    public Transform patrolPointsParent;
    public Transform[] patrolPoints;
    public int currentPatrolIndex = 0;

    public EnemyState state;

	// Use this for initialization
	void Start() { 
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		NoiseManager.instance.OnNoiseMade.AddListener(HandleNoiseEvent);
        

        patrolPoints = new Transform[patrolPointsParent.childCount];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = patrolPointsParent.GetChild(i);
        }
        //patrolPoints = patrolPointsParent.GetComponentsInChildren<Transform>();

        StartPatrolling();
	}
	
	void Destroy() {
		NoiseManager.instance.OnNoiseMade.RemoveListener(HandleNoiseEvent);
	}
	
	void FixedUpdate() {
        switch (state) {
            case EnemyState.Patrolling:
                UpdatePatrolling();
                break;
            case EnemyState.Chasing:
                UpdateChasing();
                break;
            case EnemyState.Searching:
                UpdateSearching();
                break;
        }
	}

    void UpdateChasing() {
        bool reachedDestination = MoveAlongPath();
        if (reachedDestination) {
            currentNoiseOfTarget = 0;
            state = EnemyState.Searching;
        }
        currentNoiseOfTarget = Mathf.Max(0, currentNoiseOfTarget - decayRate * Time.fixedDeltaTime);
    }

    void UpdatePatrolling() {
        bool reachedDestination = MoveAlongPath();
        if (reachedDestination)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Length)
                currentPatrolIndex = 0;

            SetDestination(patrolPoints[currentPatrolIndex].position);
        }

    }

    void StartPatrolling()
    {
        state = EnemyState.Patrolling;
        currentPatrolIndex = GetClosestPatrolPosition();
        SetDestination(patrolPoints[currentPatrolIndex].position);

    }

    int GetClosestPatrolPosition() {
        print("ahhh");
        float smallest = Mathf.Infinity;
        int result = -1;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, patrolPoints[i].position);
            if (dist < smallest)
            {
                smallest = dist;
                result = i;
            }
        }

        return result;
    }

    void UpdateSearching() {
        // TODO: Search before going back to patrolling!
        StartPatrolling();
    }

	bool MoveAlongPath(){
        if (currentPath == null) return false;

        if (currentWaypoint == currentPath.vectorPath.Count) {
            currentPath = null;
            return true;
        }

		Vector3 dir = (currentPath.vectorPath[currentWaypoint]-transform.position).normalized;
		rb.AddForce(dir * speed);

		if ((transform.position-currentPath.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
            currentWaypoint++;
        }

        return false;
	}

	void OnPathComplete(Path p) {
		currentPath = p;
		currentWaypoint = 1;
	}

	void SetDestination(Vector2 newDest){
		if (currentDestination.x != newDest.x || currentDestination.y != newDest.y){
			seeker.StartPath(transform.position, newDest, OnPathComplete);
			currentDestination = newDest;
		}
	}

    void ChaseNoise(NoiseEventData noise) {
        state = EnemyState.Chasing;
        SetDestination(noise.position);
        currentNoiseOfTarget = noise.PercievedNoise(transform.position);
    }

	public void HandleNoiseEvent(NoiseEventData noise){
        if (noise.PercievedNoise(transform.position) > currentNoiseOfTarget) {
            ChaseNoise(noise);
		}

	}

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }


}
