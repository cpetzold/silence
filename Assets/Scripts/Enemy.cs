using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public enum EnemyState {
    Patrolling, Chasing, Searching
}

public class Enemy : MonoBehaviour {

	public float chaseSpeed = 20;
    public float patrolSpeed = 15;
    public float searchSpeed = 10;
	public float nextWaypointDistance = 3;
	public float decayRate = 5;

	Vector2 currentDestination;
	public float currentNoiseOfTarget;

	Rigidbody2D rb;
	Path currentPath;
	int currentWaypoint;
	Seeker seeker;

    public Transform patrolPointsParent;
    public Vector2[] patrolPoints;
    public int currentPatrolIndex = 0;

    public LayerMask searchLayerMask;
    public Vector2[] searchPoints;
    public int minSearchPoints = 3;
    public int maxSearchPoints = 10;
    public int currentSearchIndex = 0;
    public float maxSearchDistance = 2.0f;
    public float searchWallPadding = 0.5f;

    public EnemyState state;

	void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

        if (patrolPointsParent == null)
            patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolPoints").gameObject.transform;

        patrolPoints = patrolPointsParent.GetChildren().Select(t => (Vector2)t.position).ToArray();

		NoiseManager.instance.OnNoiseMade.AddListener(HandleNoiseEvent);
        
        if (state == EnemyState.Patrolling) {
            StartPatrolling();
        } else if (state == EnemyState.Searching) {
            StartSearching();
        }
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
        bool reachedDestination = MoveAlongPath(chaseSpeed);
        if (reachedDestination) {
            currentNoiseOfTarget = 0;
            StartSearching();
        }
        currentNoiseOfTarget = Mathf.Max(0, currentNoiseOfTarget - decayRate * Time.fixedDeltaTime);
    }

    void UpdatePatrolling() {
        bool reachedDestination = MoveAlongPath(patrolSpeed);
        if (reachedDestination || currentPath == null) {
            currentPatrolIndex = Random.Range(0, patrolPoints.Length);

            SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }

    void StartPatrolling() {
        state = EnemyState.Patrolling;
        currentPatrolIndex = patrolPoints.IndexOfClosest(transform.position);
        SetDestination(patrolPoints[currentPatrolIndex]);
    }

    void UpdateSearching() {
        bool reachedDestination = MoveAlongPath(searchSpeed);
        if (reachedDestination) {
            if (currentSearchIndex == searchPoints.Length - 1) {
                StartPatrolling();
            } else {
                SetDestination(searchPoints[++currentSearchIndex], 0.5f);
            }
        }
        
        foreach (Vector2 p in searchPoints) {
            DebugExtension.DebugPoint(p, Color.red, 0.2f);
        }
    }

    void StartSearching() {
        state = EnemyState.Searching;

        int numPoints = Random.Range(minSearchPoints, maxSearchPoints);
        searchPoints = GetSearchPoints(numPoints);
        currentSearchIndex = 0;
        SetDestination(searchPoints[currentSearchIndex], 1);
    }

    Vector2[] GetSearchPoints(int numPoints) {
        Vector2[] result = new Vector2[numPoints];

        for (int i = 0; i < numPoints; i++) {
            float angle = Random.Range(0.0f, 360.0f);
            Vector2 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxSearchDistance, searchLayerMask);
            result[i] = (Vector2)transform.position + (dir * (hit.collider != null ? hit.distance - searchWallPadding : maxSearchDistance));
        }

        return result;
    }

	bool MoveAlongPath(float speed) {
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

    void SetDestination(Vector2 newDest, float delay) {
        StartCoroutine(SetDestinationCo(newDest, delay));
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

    IEnumerator SetDestinationCo(Vector2 newDest, float delay) {
        yield return new WaitForSeconds(delay);
        SetDestination(newDest);
    }
}
