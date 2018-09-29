using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour {

    public float timeToComplete;

    [HideInInspector]
    public float progressTimer;
    [HideInInspector]
    public bool inUse;

	// Use this for initialization
	void Start () {

        inUse = false;

        progressTimer = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void UseStation()
    {
        if (inUse)
        {
            if (progressTimer < timeToComplete)
                progressTimer += Time.deltaTime;
            else if (progressTimer >= timeToComplete)
            {
                CompleteStation();
                inUse = false;
                progressTimer = 0.0f;
            }
        }


    }

    public virtual void CompleteStation()
    {

    }
}
