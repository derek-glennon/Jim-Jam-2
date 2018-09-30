using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndFade : MonoBehaviour {

    private float tParamFade;
    private float tParamFloat;

    public float fadeSpeed;
    public float floatSpeed;

    private float targetY;
    private float startY;

    private float yPos;
    private float newAlpha;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {

        tParamFade = 0.0f;
        tParamFloat = 0.0f;

        targetY = transform.position.y + 2f;
        startY = transform.position.y;

        spriteRenderer = GetComponent<SpriteRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

        yPos = EaseOutQuad(startY, targetY, tParamFloat);
        tParamFloat += Time.deltaTime * floatSpeed;

        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);


        newAlpha = EaseOutQuad(1.0f, 0.0f, tParamFade);
        tParamFade += Time.deltaTime * fadeSpeed;

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

        if (newAlpha <= 0.05f)
            Destroy(gameObject);

	}


    public static float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }
}
