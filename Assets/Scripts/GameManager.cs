using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Tilemaps;

using Rewired;

public class GameManager : MonoBehaviour {
    public Color[] playerColors;

    public Enemy enemyPrefab;
    public PlayerController playerPrefab;

    public Tilemap level;

    PlayerController[] playerControllers;
    List<Enemy> enemies;

    public AnimationCurve enemiesForGoals;

    int goalsCollected;

    Player p1rewired;
    Player p2rewired;
    Player p3rewired;
    Player p4rewired;

    public InteractableItem[] itemPrefabs;
    public List<InteractableItem> items;

    public float itemTimeout = 10;
    public int maxItems = 5;

    public Text score;

    public static GameManager instance;
	// Use this for initialization
	void Start () {
        instance = this;
        playerControllers = new PlayerController[4];
        p1rewired = ReInput.players.GetPlayer(0);
        p2rewired = ReInput.players.GetPlayer(1);
        p3rewired = ReInput.players.GetPlayer(2);
        p4rewired = ReInput.players.GetPlayer(3);

        enemies = new List<Enemy>();
        items = new List<InteractableItem>();
        SpawnEnemy();
        SpawnItem();
        SpawnItem();
        SpawnItem();

        StartCoroutine(SpawnItemCo());
	}
	
	// Update is called once per frame
	void Update () {
        if (p1rewired.GetButtonDown("Start") && playerControllers[0] == null)
        {
            playerControllers[0] = SpawnPlayer(0, playerColors[0]);
        }

        if (p2rewired.GetButtonDown("Start") && playerControllers[1] == null)
        {
            playerControllers[1] = SpawnPlayer(1, playerColors[1]);
        }

        if (p3rewired.GetButtonDown("Start") && playerControllers[2] == null)
        {
            playerControllers[2] = SpawnPlayer(2, playerColors[2]);
        }

         if (p4rewired.GetButtonDown("Start") && playerControllers[3] == null)
        {
            playerControllers[3] = SpawnPlayer(3, playerColors[3]);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
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
        newPlayer.SetPlayerIndex(playerIndex);
        newPlayer.GetComponent<SpriteRenderer>().color = col;
        return newPlayer;
    }

    void SpawnItem() {
        InteractableItem item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
        item.transform.position = GetRandomUnoccupedPosition();
        items.Add(item);
    }

    public void CollectGoal()
    {
        goalsCollected += 1;
        score.text = goalsCollected.ToString();

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

    IEnumerator SpawnItemCo() {
        while (true) {
            yield return new WaitForSeconds(itemTimeout);
            if (items.Count < maxItems) {
                SpawnItem();
            }
        }
    }

}
