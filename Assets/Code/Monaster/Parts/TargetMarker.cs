using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public GameObject Target;
    public const float Y_POSITION = 0.05f;
    public float ROTATE_SPEED = 90f;
    public Material mat;
    public Renderer ren;

    public enum TargettingMode
    {
        IDLE, TARGETTING, FIRING
    }

    public TargettingMode CurrentTargettingMode = TargettingMode.TARGETTING;

    // Use this for initialization
    void Start()
    {
        ren = this.GetComponent<Renderer>();
        mat = ren.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }

        if (CurrentTargettingMode == TargettingMode.IDLE || CurrentTargettingMode == TargettingMode.TARGETTING)
        {
            if (GameController.obj.IsInRangeOfRocket)
            {
                if (!ren.enabled)
                {
                    ren.enabled = true;
                }
            }
            else
            {
                if (ren.enabled)
                {
                    ren.enabled = false;
                }
            }
        }
        else
        {
            if (!ren.enabled)
            {
                ren.enabled = true;
            }
        }

        this.transform.Rotate(0, ROTATE_SPEED * Time.deltaTime, 0);

        switch (CurrentTargettingMode)
        {
            case TargettingMode.IDLE:
                mat.color = Color.white;
                break;
            case TargettingMode.TARGETTING:
                mat.color = Color.yellow;
                break;
            case TargettingMode.FIRING:
                mat.color = Color.red;
                break;
        }

        if (GameController.obj.RocketsInFlight != 0)
        {
            //A rocket is in flight, keep target where it is.
            CurrentTargettingMode = TargettingMode.FIRING;
            return;
        }
        CurrentTargettingMode = TargettingMode.IDLE;

        Vector3 pos = Target.transform.position;
        pos.y = Y_POSITION;
        this.transform.localPosition = pos;
    }
}
