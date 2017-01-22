using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCamera : MonoBehaviour
{
    public Transform Target;
    public Transform TargetToRotate;
    public float DesiredDistance = 25f;
    public float Smoothing = 0.1f;

    private Quaternion desiredRotation;
    private float desiredHeight;
    private bool atDesiredLocation = false;

    void Start()
    {
        var follow = GetComponent<FollowTarget>();
        if (follow != null)
        {
            follow.enabled = false;
        }

        desiredRotation = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
        desiredHeight = Target.position.y;

        float angle = 60f + Mathf.Sign(transform.position.x) * Vector3.Angle(Vector3.forward, transform.position);
        if (TargetToRotate != null)
        {
            TargetToRotate.rotation = Quaternion.Euler(
                TargetToRotate.rotation.eulerAngles.x,
                angle,
                TargetToRotate.rotation.eulerAngles.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!atDesiredLocation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Smoothing);
            //transform.position = Vector3.Lerp(transform.position, Target.position, Smoothing);

            if (Mathf.Abs(transform.position.y - Target.position.y) > 0.3f)
            {
                float newY = Mathf.Lerp(transform.position.y, desiredHeight, Smoothing);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                atDesiredLocation = true;
            }
        }
        else
        {
            if ((Target.position - transform.position).magnitude > DesiredDistance)
            {
                transform.position = Vector3.Lerp(transform.position, Target.position, Smoothing);
            }
            var midHeight = Target.position / 2;
            var lookAtRotation = Quaternion.LookRotation(midHeight - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, Smoothing);
        }
    }
}
