using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [HideInInspector]
    public bool canMove;

    public float speed;

    public bool holdingItem;

    public Item heldItem;

    public bool usingStation;
    public Station stationBeingUsed;

    public float pickUpBufferTimerInit;
    private float pickUpBufferTimer;
    private bool isPickUpBufferOn;

    public float dropBufferTimerInit;
    private float dropBufferTimer;
    private bool isDropBufferOn;

    public float stationUseBufferTimerInit;
    public float stationUseBufferTimer;

    private bool isMoving;
    private bool isFacingRight;

    public bool inStationArea;

    public OuroborosSystem ouroborosSystem;



	// Use this for initialization
	void Start () {

        inStationArea = false;

        isPickUpBufferOn = false;
        pickUpBufferTimer = pickUpBufferTimerInit;
        isDropBufferOn = false;
        //dropBufferTimer = dropBufferTimerInit;
        dropBufferTimer = 0.0f;


        stationUseBufferTimer = stationUseBufferTimerInit;

        ouroborosSystem = GameObject.Find("Ouroboros System").GetComponent<OuroborosSystem>();

        canMove = true;
        holdingItem = false;
        usingStation = false;
	}

    private void Update()
    {
        //Quit the Game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        //Get Input
        float horizontal = 0.0f;
        float vertical = 0.0f;
        if (canMove)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        //Set IsMoving
        if (horizontal != 0)
        {
            isMoving = true;
        }
        else if (horizontal == 0)
        {
            isMoving = false;
        }

        //If there is input, move the player
        if (horizontal != 0)
            GetComponent<Transform>().Translate(new Vector3(horizontal * speed, 0.0f, 0.0f) * Time.deltaTime);
        if (vertical != 0)
            GetComponent<Transform>().Translate(new Vector3(0.0f, vertical * speed, 0.0f) * Time.deltaTime);

        //Flip the player direction if movement is changed
        if (horizontal > 0 && !isFacingRight)
            Flip();
        else if (horizontal < 0 && isFacingRight)
            Flip();

        //Pick Up Buffer
        if (isPickUpBufferOn)
            pickUpBufferTimer -= Time.deltaTime;
        if (pickUpBufferTimer < 0.0f)
            pickUpBufferTimer = 0.0f;
        //Drop Buffer
        if (isDropBufferOn)
            dropBufferTimer -= Time.deltaTime;
        if (dropBufferTimer < 0.0f)
            dropBufferTimer = 0.0f;
        //Station Use Buffer
        if (stationUseBufferTimer > 0.0f)
            stationUseBufferTimer -= Time.deltaTime;
        if (stationUseBufferTimer <= 0.0f)
            stationUseBufferTimer = 0.0f;


        //Drop Items
        if (holdingItem && Input.GetAxis("Fire1") == 1 && pickUpBufferTimer <= 0.0f && !inStationArea)
        {
            isPickUpBufferOn = false;
            pickUpBufferTimer = pickUpBufferTimerInit;

            isDropBufferOn = true;

            holdingItem = false;
            heldItem.GetComponent<Item>().isHeld = false;
            heldItem = null;
        }

        //Using Stations
        if (usingStation)
        {

            //Use the Station
            stationBeingUsed.UseStation();


            //Check if station has finished
            if (!stationBeingUsed.inUse)
            {
                usingStation = false;
                stationBeingUsed = null;
            }
        }


	}

    private void OnTriggerStay2D(Collider2D other)
    {
        //Pick Up Items
        if (other.gameObject.CompareTag("Item") && !holdingItem && Input.GetAxis("Fire1") == 1 && dropBufferTimer <= 0.0f && !other.GetComponent<Item>().inAlchemyStation)
        {

            isPickUpBufferOn = true;

            isDropBufferOn = false;
            dropBufferTimer = dropBufferTimerInit;

            holdingItem = true;
            other.GetComponent<Item>().isHeld = true;
            heldItem = other.GetComponent<Item>();
        }

        //Use Stations
        if (other.gameObject.CompareTag("Gathering Station"))
        {

            inStationArea = true;

            if (!holdingItem && Input.GetAxis("Fire1") == 1 && !usingStation && stationUseBufferTimer == 0.0f)
            {
                stationUseBufferTimer = stationUseBufferTimerInit;
                usingStation = true;
                other.GetComponent<Station>().inUse = true;
                stationBeingUsed = other.GetComponent<Station>();
            }

        }

        if (other.gameObject.CompareTag("Alchemy Station"))
        {

            inStationArea = true;

            if (Input.GetAxis("Fire1") == 1 && !usingStation && stationUseBufferTimer == 0.0f)
            {
                stationUseBufferTimer = stationUseBufferTimerInit;
                usingStation = true;
                //other.GetComponent<Station>().inUse = true;
                stationBeingUsed = other.GetComponent<Station>();
            }

        }


        //Give Ouroboros Head an Item
        if (other.gameObject.CompareTag("Ouroboros Head"))
        {
            inStationArea = true;

            if (holdingItem && Input.GetAxis("Fire1") == 1)
            {

                if (heldItem is CombinedElement)
                {
                    CombinedElement item = heldItem as CombinedElement;
                    ouroborosSystem.GiveItem(item);
                    Destroy(item.gameObject);
                    heldItem = null;
                    holdingItem = false;
                    isDropBufferOn = true;
                    pickUpBufferTimer = pickUpBufferTimerInit;
                    isPickUpBufferOn = false;
                }

            }



        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Leaving Stations
        if ((other.gameObject.CompareTag("Gathering Station") || (other.gameObject.CompareTag("Alchemy Station"))))
        {

            inStationArea = false;

            if (usingStation)
            {
                usingStation = false;
                other.GetComponent<Station>().inUse = false;
                stationBeingUsed = null;
            }

        }

        //Leaving Ouroboros Head
        if (other.gameObject.CompareTag("Ouroboros Head"))
        {
            inStationArea = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void GetItemFromStation(Item item)
    {
        holdingItem = true;
        heldItem = item;
        isPickUpBufferOn = true;
        dropBufferTimer = dropBufferTimerInit;
        isDropBufferOn = false;
    }

    public void DropItemAtStation(Item item)
    {
        holdingItem = false;
        heldItem = null;
        isDropBufferOn = true;
        pickUpBufferTimer = pickUpBufferTimerInit;
        isPickUpBufferOn = false;
    }


}
