using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

public class RandomGoal : MonoBehaviour {

    public Tilemap level;

    public int x;
    public int y;

	// Use this for initialization
	void Start () {
        MoveGoalToRandomPosition();

        TilemapCollider2D collider = level.GetComponent<TilemapCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    [Button]
    void MoveGoalToRandomPosition() {
        TileBase tile;
        int randx;
        int randy;

        do {
            randx = Random.Range(level.cellBounds.xMin+1, level.cellBounds.xMax-1);
            randy = Random.Range(level.cellBounds.yMin+1, level.cellBounds.yMax-1);

            tile = level.GetTile(new Vector3Int(randx, randy, 0));
        } while (tile != null);

        transform.position = level.CellToWorld(new Vector3Int(randx, randy, 0)) + (level.cellSize / 2);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        MoveGoalToRandomPosition();
    }

}
