﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

public class RandomGoal : MonoBehaviour {

    public Tilemap level;

    public int x;
    public int y;

	void Start () {
        MoveGoalToRandomPosition();
	}

    [Button]
    void MoveGoalToRandomPosition() {
        Tile.ColliderType colliderType;
        int randx;
        int randy;

        do {
            randx = Random.Range(level.cellBounds.xMin+1, level.cellBounds.xMax-1);
            randy = Random.Range(level.cellBounds.yMin+1, level.cellBounds.yMax-1);

            colliderType = level.GetColliderType(new Vector3Int(randx, randy, 0));
        } while (colliderType != Tile.ColliderType.None);

        transform.position = level.CellToWorld(new Vector3Int(randx, randy, 0)) + (level.cellSize / 2);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            GameManager.instance.CollectGoal();
            MoveGoalToRandomPosition();
        }
    }

}
