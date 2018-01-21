using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {

    public Enemy enemyPrefab;
    public PlayerController playerPrefab;

    public Tilemap tilemap;

    PlayerController[] playerControllers;
    Enemy[] enemies;

    int goalsCollected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnEnemy()
    {

    }

    public void CollectGoal()
    {
        goalsCollected += 1;


    }

}
