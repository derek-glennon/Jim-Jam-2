using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadMover : MonoBehaviour {

    public float radius;

    private float posX, posY, angle = 0.0f;

    public Image BodyImage;

	// Use this for initialization
	void Start () {

        SetPosition();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        SetPosition();


	}


    public void SetPosition()
    {
        float percentFilled = BodyImage.fillAmount;

        float angle = ((percentFilled * 360.0f) + 90.0f) * (Mathf.PI / 180.0f);

        posX = radius * Mathf.Cos(angle);
        posY = radius * Mathf.Sin(angle);

        transform.position = new Vector2(posX, posY);
    }
}
