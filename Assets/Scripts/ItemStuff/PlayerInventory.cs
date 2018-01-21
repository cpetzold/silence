using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class PlayerInventory : MonoBehaviour {

    public InventoryItem currentItem;
    public int numUses;

    Player player;
    PlayerController controller;

    List<InteractableObject> nearbyItems;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        player = ReInput.players.GetPlayer(controller.playerIndex);

        nearbyItems = new List<InteractableObject>();
    }

	// Update is called once per frame
	void Update () {

        if (player.GetButtonDown("YELL"))
        {
            if (currentItem != null)
            {
                currentItem.Use(this);
                
                numUses -= 1;

                if(numUses <= 0)
                    currentItem = null;
                
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
            return true;
        }
        else
        {
            return false;
        }
    }
}
