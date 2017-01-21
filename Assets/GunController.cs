using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    const float TOLERANCE = 0.1f;

    private static Vector2 stickUp = new Vector2(0, 1);

    public Transform Gun;
    public float RotationSpeed = 100f;

    void Awake() {
        
    }
    
    void Update ()
    {
        //float deltaTime = Time.deltaTime;

        //Gun.Rotate(transform.forward, RotationSpeed * deltaTime);

        //float h = Input.GetAxis("Joystick X");
        //float v = Input.GetAxis("Joystick Y");

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //var direction = new Vector3(h, v, 0f);
        //float angle = Vector2.Angle(stickUp, direction);

        //if (Mathf.Abs(h) > TOLERANCE || Mathf.Abs(v) > TOLERANCE)
        //{
        //    Debug.Log("h/v: " + h + "," + v);
        //    Debug.Log("Angle: " + angle);
        //}
    }
}
