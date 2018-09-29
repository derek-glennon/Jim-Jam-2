using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int elementType;//0 = fire, 1 = air, 2 = water, 3 = earth

    public bool isHeld;

    public bool inAlchemyStation;

    private Transform itemPosition;

    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    public virtual void Start () {

        inAlchemyStation = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemPosition = GameObject.Find("Item Position").GetComponent<Transform>();

	}

    public virtual void FixedUpdate()
    {

        if (isHeld)
        {
            transform.position = itemPosition.position;
        }
    }

    // Update is called once per frame
    void Update () {


    }
}
