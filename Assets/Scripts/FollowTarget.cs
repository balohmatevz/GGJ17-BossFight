using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;

    public float DesiredDistance = 15f;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            float newZ = Target.position.z - DesiredDistance;
            transform.position = new Vector3(Target.position.x, transform.position.y, newZ);
        }
    }
}
