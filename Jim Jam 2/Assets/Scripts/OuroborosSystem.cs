using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OuroborosSystem : MonoBehaviour {

    public Image bodyImage;

    private GameManager gameManager;

    private float startingFillAmount;
    public float fillAmount;
    private float targetFillAmount;
    private float startGrowAmount;

    private float tParam = 0.0f;
    public float growSpeed;

    private float tParamReset = 0.0f;
    public float resetSpeed;
    public bool isResetting;
    private float startResetAmount;
    private float endResetAmount;

    public int correctElement1; //0 = fire, 1 = air, 2 = water, 3 = earth
    public int correctElement2; //0 = fire, 1 = air, 2 = water, 3 = earth

    public float oneCorrectPointAmount;
    public float bonusPointAmount;

    private bool isGrowing;

    private bool atTail;

    public bool foundCombo;

    public GameObject smallHeart;
    public GameObject bigHeart;
    public GameObject sadFace;


    public AudioClip oneCorrectClip;
    public AudioClip bothCorrectClip;
    public AudioClip noneCorrectClip;
    public AudioClip completeClip;

    private AudioSource audioSource;

    float value = 0.0f;

    // Use this for initialization
    void Awake () {


        atTail = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();

        isGrowing = false;

        isResetting = false;

	}

    public void SetStartingHeadValue(float startingHeadValue)
    {
        startingFillAmount = startingHeadValue;

        if (!isResetting)
        { 

            fillAmount = startingFillAmount;
            bodyImage.fillAmount = fillAmount / 100.0f;

            targetFillAmount = fillAmount;
            startGrowAmount = fillAmount;
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (fillAmount >= 98.0f && isGrowing && !isResetting)
        {
            atTail = true;
            fillAmount = 100.0f;
            audioSource.clip = completeClip;
            audioSource.Play();
            StopGrowing();
        }
            

        if (fillAmount < targetFillAmount && isGrowing)
        {
            fillAmount = EaseOutQuad(startGrowAmount, targetFillAmount, tParam);
            tParam += Time.deltaTime * growSpeed;

            bodyImage.fillAmount = fillAmount / 100.0f;
        }

        if (fillAmount >= (targetFillAmount - 0.1f )  && isGrowing)
            StopGrowing();


        if (isResetting && fillAmount > startingFillAmount)
        {
            fillAmount = EaseOutQuad(startResetAmount, endResetAmount, tParamReset);
            tParamReset += Time.deltaTime * resetSpeed;

            bodyImage.fillAmount = fillAmount / 100.0f;
        }

        if (isResetting && fillAmount < (endResetAmount + 0.1f))
            StopResetting();

        
		
	}

    public void GiveItem(CombinedElement combinedElement)
    {

        int numberCorrect = 0;

        //If the element is mixed
        if (combinedElement.elementTypes[0] != combinedElement.elementTypes[1])
        {

            if (combinedElement.elementTypes[0] == correctElement1 || combinedElement.elementTypes[0] == correctElement2)
                numberCorrect += 1;

            if (combinedElement.elementTypes[1] == correctElement1 || combinedElement.elementTypes[1] == correctElement2)
                numberCorrect += 1;
        }
        //If the combined element is the same element twice
        else
        {
            if (combinedElement.elementTypes[0] == correctElement1 || combinedElement.elementTypes[0] == correctElement2)
                numberCorrect += 1;
        }


        if (numberCorrect == 0)
            EmitSadFace();

        if (numberCorrect == 1)
        {
            targetFillAmount += oneCorrectPointAmount;
            SetGrowing();
            EmitSmallHeart();
        }


        if (numberCorrect == 2)
        {
            targetFillAmount += 2 * oneCorrectPointAmount;
            targetFillAmount += bonusPointAmount;
            SetGrowing();
            EmitBigHeart();
            foundCombo = true;
        }

    }

    void SetGrowing()
    {

        if (!isGrowing)
        {
            isGrowing = true;

            startGrowAmount = fillAmount;
        }
    }

    void StopGrowing()
    {
        if (atTail)
            fillAmount = 100.0f;
        else
            fillAmount = targetFillAmount;

        tParam = 0.0f;
        isGrowing = false;
        
        startGrowAmount = targetFillAmount;
        bodyImage.fillAmount = fillAmount / 100.0f;
    }

    public void StartResetting()
    {
        atTail = false;

        if (!isResetting)
        {
            isResetting = true;

            startResetAmount = 100.0f;

            endResetAmount = startingFillAmount;
        }
    }

    void StopResetting()
    {
        fillAmount = endResetAmount;

        tParamReset = 0.0f;
        isResetting = false;

        targetFillAmount = fillAmount;
        startGrowAmount = fillAmount;

        bodyImage.fillAmount = fillAmount / 100.0f;
    }

    void EmitSmallHeart()
    {
        Instantiate(smallHeart, transform.position, Quaternion.identity);
        audioSource.clip = oneCorrectClip;
        audioSource.Play();
    }

    void EmitBigHeart()
    {
        Instantiate(bigHeart, transform.position, Quaternion.identity);
        audioSource.clip = bothCorrectClip;
        audioSource.Play();
    }

    void EmitSadFace()
    {
        Instantiate(sadFace, transform.position, Quaternion.identity);
        audioSource.clip = noneCorrectClip;
        audioSource.Play();
    }

    public static float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    public static float EaseInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    }
}
