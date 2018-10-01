using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartStation : Station {

    // Use this for initialization
    public override void Start () {

        base.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public override void CompleteStation()
    {
        base.CompleteStation();
        SceneManager.LoadScene(1);
    }

}
