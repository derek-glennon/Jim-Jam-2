using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGlow : MonoBehaviour {

    private Light myLight;

    public float speed;
    public float highValue;
    public float lowValue;

	// Use this for initialization
	void Awake() {

        myLight = GetComponent<Light>();
		
	}
	
	// Update is called once per frame
	void Update () {

        myLight.intensity = lowValue + Mathf.PingPong(Time.time * speed, highValue);
		
	}
}
