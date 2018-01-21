using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem")]
public class InventoryItem : ScriptableObject {

    public Texture icon;
    public int numUses;

    public virtual void Use(Transform userTransform)
    {
        Debug.Log("test use");
    }
}
