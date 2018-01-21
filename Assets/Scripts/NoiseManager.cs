using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NoiseEventData{
	public Vector2 position;
	public float radius;

	public NoiseEventData(Vector2 loc, float rad){
		position = loc;
		radius = rad;
	}

    public float PercievedNoise(Vector2 listenerPosition)
    {
        float dist = Vector2.Distance(listenerPosition, position);
        return 1 - (dist / radius);
    }
	
}

[System.Serializable]
public class NoiseEvent : UnityEvent<NoiseEventData>{

}

public class NoiseManager : MonoBehaviour {

	public static NoiseManager instance;

	public NoiseEvent OnNoiseMade;

	public static void MakeNoise(Vector2 loc, float radius){
		instance.OnNoiseMade.Invoke(new NoiseEventData(loc, radius));
	}

	void Awake(){
		instance = this;
        OnNoiseMade = new NoiseEvent();
	}

}
