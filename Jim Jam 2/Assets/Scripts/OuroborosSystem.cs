using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OuroborosSystem : MonoBehaviour {

    public Image bodyImage;

    public float startingFillAmount;
    private float fillAmount;

    public int correctElement1; //0 = fire, 1 = air, 2 = water, 3 = earth
    public int correctElement2; //0 = fire, 1 = air, 2 = water, 3 = earth

    // Use this for initialization
    void Start () {

        fillAmount = startingFillAmount;

	}
	
	// Update is called once per frame
	void Update () {

        if (fillAmount > 100.0f)
            fillAmount = 100.0f;

        bodyImage.fillAmount = fillAmount / 100.0f;
		
	}

    public void GiveItem(CombinedElement combinedElement)
    {

        int numberCorrect = 0;

        if (combinedElement.elementTypes[0] == correctElement1 || combinedElement.elementTypes[0] == correctElement2)
        {
            fillAmount += 20f;
            numberCorrect += 1;
        }

        if (combinedElement.elementTypes[1] == correctElement1 || combinedElement.elementTypes[1] == correctElement2)
        {
            fillAmount += 20f;
            numberCorrect += 1;
        }

        if (numberCorrect == 2)
        {
            fillAmount += 10.0f;
        }
    }
}
