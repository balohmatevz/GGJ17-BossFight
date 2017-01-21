using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Constants

    const float TOLERANCE = 0.4f;

    #endregion Constants

    #region Public Members

    private static Vector2 stickUp = new Vector2(0, 1);

    [Header("References")]
    public Transform Gun;

    [Header("Prefabs")]
    public GameObject ProjectilePrefab;

    [Header("Variables")]
    public float RotationSpeed = 100f;
    public float RechargeTime = 0.1f;

    #endregion Public Members

    #region Private Members

    private float lastFireTime = 0f;
    
    #endregion Private Members
    
    void Update ()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;

        float h = Input.GetAxis("Joystick X");
        float v = Input.GetAxis("Joystick Y");

        // Add some tolerance to prevent the gun from moving when releasing the analog stick
        if (Mathf.Abs(h) > TOLERANCE || Mathf.Abs(v) > TOLERANCE)
        {
            var direction = new Vector2(h, v);
            float angle = Mathf.Sign(h) * Vector2.Angle(stickUp, direction);
            Gun.localEulerAngles = new Vector3(0, 0, angle);            
        }

        if (Input.GetAxis("Right Trigger") > 0.5f)
        {
            if (time - lastFireTime > RechargeTime)
            {
                lastFireTime = time;
                Fire();
            }
        }
    }

    private void Fire()
    {
        // TODO: Fire projectiles
    }
}
