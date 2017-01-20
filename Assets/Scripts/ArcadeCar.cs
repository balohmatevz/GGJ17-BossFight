using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ArcadeCar : MonoBehaviour
{
    private Rigidbody rigidbody;

    public ForceMode ForceMode = ForceMode.Acceleration;
    public float ForceAmount = 20f;
    public float TopSpeed = 100f;

    public float RotateSpeed = 0.1f;

    // Use this for initialization
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Move(float h, float v, float handbrake)
    {
        //Debug.Log("Adding force: forward: " + v + " l/r: " + h);
        rigidbody.AddForce(transform.forward * v * ForceAmount, ForceMode);

        float speed = rigidbody.velocity.magnitude;
        if (speed > TopSpeed)
        {
            rigidbody.velocity = TopSpeed * rigidbody.velocity.normalized;
        }

        transform.Rotate(transform.up, RotateSpeed * h * Time.fixedDeltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        // pass the input to the car!
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
        Move(h, v, handbrake);
#else
        Move(h, v, 0f);
#endif
    }
}
