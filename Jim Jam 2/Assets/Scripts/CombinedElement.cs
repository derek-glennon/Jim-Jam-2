using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedElement : Item {

    public int[] elementTypes;


    private SpriteRenderer[] spriteRenderers;

    // Use this for initialization
    public override void Start()
    {

        base.Start();

    }

    public void Awake()
    {
        int index = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        spriteRenderers = new SpriteRenderer[transform.childCount];
        elementTypes = new int[transform.childCount];

        foreach (Transform child in transform)
        {
            allChildren[index] = child.gameObject;
            spriteRenderers[index] = allChildren[index].GetComponent<SpriteRenderer>();
            index += 1;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    void Update () {

        int index = 0;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (elementTypes[index] == 0)
                spriteRenderer.color = Color.red;
            else if (elementTypes[index] == 1)
                spriteRenderer.color = Color.gray;
            else if (elementTypes[index] == 2)
                spriteRenderer.color = Color.blue;
            else if (elementTypes[index] == 3)
                spriteRenderer.color = new Color(0.5386567f, 0.8679245f, 0.4544322f, 1f);

            index += 1;
        }

		
	}
}
