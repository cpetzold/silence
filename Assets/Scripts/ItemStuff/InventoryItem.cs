using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseItem", menuName="Inventory Items/BaseItem")]
public class InventoryItem : ScriptableObject {

    public Sprite icon;
    public int numUses;

    public virtual void Use(PlayerInventory inventory)
    {

    }
}
