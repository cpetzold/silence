using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class PlayerInventory : MonoBehaviour {

    public InventoryItem currentItem;
    public int numUses;
    public SpriteRenderer itemSpriteRenderer;

    PlayerController controller;

    List<InteractableObject> nearbyItems;

    void Awake()
    {
        controller = GetComponent<PlayerController>();

        nearbyItems = new List<InteractableObject>();
    }

	// Update is called once per frame
	void Update () {

        if (controller.player.GetButtonDown("YELL"))
        {
            if (currentItem != null)
            {
                UseItem();
                
            }
            else
            {
                if (nearbyItems.Count > 0)
                {
                    InteractableObject firstObj = nearbyItems[0];
                    firstObj.Interact(this);
                }
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();

        if (interactable != null)
        {
            nearbyItems.Add(interactable);
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        print("bye");
        InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();

        if (interactable != null)
        {
            nearbyItems.Remove(interactable);
        }
    }

    public bool PickupItem(InventoryItem item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            numUses = item.numUses;
            itemSpriteRenderer.sprite = currentItem.icon;
            itemSpriteRenderer.enabled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseItem()
    {
        currentItem.Use(this);

        numUses -= 1;

        if (numUses <= 0) {
            currentItem = null;
            itemSpriteRenderer.enabled = false;
        }
    }
}
