using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : Item {



    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    void Update () {

        //Change the Element Color
        if (elementType == 0)
            spriteRenderer.color = Color.red;
        else if (elementType == 1)
            spriteRenderer.color = Color.gray;
        else if (elementType == 2)
            spriteRenderer.color = Color.blue;
        else if (elementType == 3)
            spriteRenderer.color = new Color(0.5386567f, 0.8679245f, 0.4544322f, 1f);

    }
}
