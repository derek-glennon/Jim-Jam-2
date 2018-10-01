using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyStation : Station {

    public bool haveBothElements;

    public Transform elementSlot1;
    public Transform elementSlot2;

    public Item element1;
    public Item element2;

    private PlayerController player;
    private Transform itemPosition;

    //private AudioSource audioSource;

    public AudioClip popClip;


    public GameObject result;

	// Use this for initialization
	public override void Start () {

        base.Start();

        haveBothElements = false;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        itemPosition = GameObject.Find("Item Position").GetComponent<Transform>();
        //audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void UseStation()
    {
        if (haveBothElements && player.heldItem == null)
        {
            inUse = true;
            base.UseStation();
        }
        else if (!haveBothElements)
            if (player.heldItem != null)
                PlaceItem(player.heldItem);
    }

    public void PlaceItem(Item item)
    {
        audioSource.clip = popClip;
        audioSource.Play();

        if (element1 == null)
        {
            element1 = item;
            item.isHeld = false;
            item.transform.position = elementSlot1.position;
            item.inAlchemyStation = true;
            player.DropItemAtStation(item);
        }
        else if (element1 != null && element2 == null)
        {
            element2 = item;
            item.isHeld = false;
            item.transform.position = elementSlot2.position;
            item.inAlchemyStation = true;
            player.DropItemAtStation(item);

            //We have both elements!
            haveBothElements = true;
        }
    }

    public override void CompleteStation()
    {

        base.CompleteStation();

        GameObject clone;
        clone = Instantiate(result, itemPosition.position, Quaternion.identity) as GameObject;
        clone.GetComponent<Item>().isHeld = true;
        clone.GetComponent<CombinedElement>().elementTypes[0] = element1.elementType;
        clone.GetComponent<CombinedElement>().elementTypes[1] = element2.elementType;
        player.GetItemFromStation(clone.GetComponent<Item>());
        Destroy(element1.gameObject);
        Destroy(element2.gameObject);
        element1 = null;
        element2 = null;
        haveBothElements = false;
    }
}
