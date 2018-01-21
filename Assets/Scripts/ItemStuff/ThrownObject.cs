using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour {

    public LayerMask layerMask;

    public float noiseAmount = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            NoiseManager.MakeNoise(transform.position, noiseAmount);
            Destroy(this.gameObject);
        }
        
    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }


}
