using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPart : MonoBehaviour
{
    public const float BLINK_SPEED = 0.2f;
    public static Color BLINK_COLOR = Color.white;
    public const float BLINK_DURATION = 2f;

    public float Health = 100;
    public bool CanBeHit = true;
    public Material mat;

    public Color OriginalColor = Color.white;
    public bool IsBlinkColorChanged = false;
    public float HitTimer = 0f;
    public float BlinkTimer = 0f;

    // Use this for initialization
    void Start()
    {
        Renderer rend = this.GetComponent<Renderer>();
        if (rend != null)
        {
            mat = this.GetComponent<Renderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HitTimer > 0)
        {
            HitTimer -= Time.deltaTime;
            BlinkTimer -= Time.deltaTime;

            if (BlinkTimer <= 0)
            {
                //Swap blink color
                BlinkTimer = BLINK_SPEED;
                if (IsBlinkColorChanged)
                {
                    //Blinking from original to red
                    IsBlinkColorChanged = false;
                    mat.color = BLINK_COLOR;
                }
                else
                {
                    //Blinking from red to original
                    IsBlinkColorChanged = true;
                    mat.color = OriginalColor;
                }
            }

            if (HitTimer <= 0)
            {
                //Blinking over
                mat.color = OriginalColor;
            }
        }
    }

    public virtual void OnHit()
    {
        if (HitTimer > 0)
        {
            //Already blinking
            return;
        }

        OriginalColor = mat.color;
        HitTimer = BLINK_DURATION;
        IsBlinkColorChanged = true;
        BlinkTimer = BLINK_SPEED;
        mat.color = BLINK_COLOR;
    }
}
