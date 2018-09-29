using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class ProgressBar : MonoBehaviour {

    public Station station;

    private Image progressBar;

	// Use this for initialization
	void Start () {

        station = GetComponentInParent<Station>();

        progressBar = GetComponent<Image>();
		
	}
	
	// Update is called once per frame
	void Update () {

        progressBar.fillAmount = (station.progressTimer / station.timeToComplete);
		
	}
}
