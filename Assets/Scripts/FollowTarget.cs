using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;

    public float DesiredDistance = 15f;
    //public float Smoothing = 0.6f;

    void Awake()
    {
            
    }
    
    // Update is called once per frame
    void Update ()
    {
        float newZ;
        float desiredZ = newZ = Target.position.z - DesiredDistance;
        //float newZ = Mathf.Lerp(transform.position.z, desiredZ, Smoothing);
        transform.position = new Vector3(Target.position.x, transform.position.y, newZ);
    }
}
