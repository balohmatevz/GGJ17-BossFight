using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public GameObject Target;
    public const float Y_POSITION = 0.05f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }

        if (GameController.obj.RocketsInFlight != 0)
        {
            //A rocket is in flight, keep target where it is.
            return;
        }

        Vector3 pos = Target.transform.position;
        pos.y = Y_POSITION;
        this.transform.localPosition = pos;
    }
}
