using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : InteractableObject {

    public InventoryItem inventoryItem;

    public override void Interact(PlayerInventory playerInventory)
    {
        if (playerInventory.PickupItem(inventoryItem))
        {
            Destroy(this.gameObject);
        }
    }
}
