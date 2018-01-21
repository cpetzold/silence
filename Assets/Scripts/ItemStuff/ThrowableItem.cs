using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseItem", menuName = "Inventory Items/ThrowableItem")]
public class ThrowableItem : InventoryItem {

    public GameObject thrownItemPrefab;
    public float throwSpeed;

    public override void Use(PlayerInventory inventory)
    {
       
        GameObject thrownItem = Instantiate(thrownItemPrefab);
        Rigidbody2D rb = thrownItem.GetComponent<Rigidbody2D>();

        thrownItem.transform.position = inventory.transform.position;

        Vector2 dir = inventory.transform.up;
        rb.velocity = dir * throwSpeed;

        Debug.Log(rb.velocity);
        
    }
	
}
