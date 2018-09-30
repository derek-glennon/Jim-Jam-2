using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringStation : Station {

    public int element; //0 = fire, 1 = air, 2 = water, 3 = earth


    public Sprite fireSprite;
    public Sprite airSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;

    //private Light glowLight;

    public GameObject item;

    public SpriteRenderer spriteRenderer;

    private PlayerController player;
    private Transform itemPosition;

    // Use this for initialization
    void Start () {

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        itemPosition = GameObject.Find("Item Position").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //glowLight = GetComponentInChildren<Light>();

    }

    // Update is called once per frame
    void Update () {

        //Change the Gathering Station Color
        if (element == 0)
        {
            spriteRenderer.sprite = fireSprite;
            spriteRenderer.color = Color.red;
            //glowLight.color = Color.red;
        }
        else if (element == 1)
        {
            spriteRenderer.sprite = airSprite;
            spriteRenderer.color = Color.gray;
            //glowLight.color = Color.gray;
        }
        else if (element == 2)
        {
            spriteRenderer.sprite = waterSprite;
            spriteRenderer.color = Color.blue;
            //glowLight.color = Color.blue;
        }
        else if (element == 3)
        {
            spriteRenderer.sprite = earthSprite;
            spriteRenderer.color = new Color(0.5386567f, 0.8679245f, 0.4544322f, 1f);
            //glowLight.color = new Color(0.5386567f, 0.8679245f, 0.4544322f, 1f);
        }
            
        //spriteRenderer.color = new Color(0.5386567f, 0.8679245f, 0.4544322f, 1f);

    }

    public override void CompleteStation()
    {
        GameObject clone;
        clone = Instantiate(item, itemPosition.position, Quaternion.identity) as GameObject;
        clone.GetComponent<Item>().isHeld = true;
        clone.GetComponent<Item>().elementType = element;
        player.GetItemFromStation(clone.GetComponent<Item>());

    }
}
