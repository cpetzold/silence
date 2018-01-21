using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

   // public float interactionDist;

    CircleCollider2D triggerCollider;

    void Awake()
    {
       // triggerCollider = GetComponent<CircleCollider2D>();
        //triggerCollider.radius = interactionDist;
    }

    public virtual void Interact(PlayerInventory playerInventory)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //print("item trigger entered");
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
       // print("item trigger exited");
    }


	
}
