using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarCanvas : MonoBehaviour {

    public Station station;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {

        station = GetComponentInParent<Station>();

        rectTransform = GetComponent<RectTransform>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!station.inUse)
        {
            rectTransform.localPosition= new Vector3(100.0f, 100.0f, 0.0f);
        }
        else if (station.inUse)
        {
            rectTransform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
		
	}
}
