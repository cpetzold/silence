using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airhorn : MonoBehaviour {

    public float noiseAmount = 30;

	// Use this for initialization
	void Start () {
        NoiseManager.MakeNoise(transform.position, noiseAmount);
        Destroy(this.gameObject);
	}
	
}
