using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

using Rewired;

public class GameManager : MonoBehaviour {
    public Color p1Color;
    public Color p2Color;

    public Enemy enemyPrefab;
    public PlayerController playerPrefab;

    public Tilemap level;

    PlayerController[] playerControllers;
    List<Enemy> enemies;

    public AnimationCurve enemiesForGoals;

    int goalsCollected;

    Player p1rewired;
    Player p2rewired;

    public static GameManager instance;
	// Use this for initialization
	void Start () {
        instance = this;
        playerControllers = new PlayerController[4];
        p1rewired = ReInput.players.GetPlayer(0);
        p2rewired = ReInput.players.GetPlayer(1);

        enemies = new List<Enemy>();
        SpawnEnemy();
	}
	
	// Update is called once per frame
	void Update () {
        if (p1rewired.GetButtonDown("Start") && playerControllers[0] == null)
        {
            playerControllers[0] = SpawnPlayer(0, p1Color);
        }

        if (p2rewired.GetButtonDown("Start") && playerControllers[1] == null)
        {
            playerControllers[1] = SpawnPlayer(1, p2Color);
        }

    }

    void SpawnEnemy()
    {
        Enemy newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = GetRandomUnoccupedPosition();
        enemies.Add(newEnemy);
    }

    PlayerController SpawnPlayer(int playerIndex, Color col)
    {
        PlayerController newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.position = GetRandomUnoccupedPosition();
        newPlayer.playerIndex = playerIndex;
        newPlayer.GetComponent<SpriteRenderer>().color = col;
        return newPlayer;
    }

    public void CollectGoal()
    {
        goalsCollected += 1;

        int numDesiredEnemies = (int)Mathf.Floor(enemiesForGoals.Evaluate(goalsCollected));

        while(enemies.Count < numDesiredEnemies)
        {
            SpawnEnemy();
        }

    }

    public Vector2 GetRandomUnoccupedPosition()
    {
        Tile.ColliderType colliderType;
        int randx;
        int randy;

        do
        {
            randx = Random.Range(level.cellBounds.xMin + 1, level.cellBounds.xMax - 1);
            randy = Random.Range(level.cellBounds.yMin + 1, level.cellBounds.yMax - 1);

            colliderType = level.GetColliderType(new Vector3Int(randx, randy, 0));
        } while (colliderType != Tile.ColliderType.None);

        return level.CellToWorld(new Vector3Int(randx, randy, 0)) + (level.cellSize / 2);
    }

}
