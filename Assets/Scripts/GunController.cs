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
    public ParticleSystem GunParticleSystem;

    [Header("Variables")]
    public float RotationSmoothing = 0.1f;
    public bool IsShooting = false;

    #endregion Public Members
    
    void Update()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;

        float h = Input.GetAxis("Joystick X");
        float v = Input.GetAxis("Joystick Y");        

        // Add some tolerance to prevent the gun from moving when releasing the analog stick
        if (Mathf.Abs(h) > TOLERANCE || Mathf.Abs(v) > TOLERANCE)
        {
            var lastRotation = Gun.rotation;

            var direction = new Vector2(h, v);
            float newAngle = Mathf.Sign(h) * Vector2.Angle(stickUp, direction);
            var desiredAngles = new Vector3(0, newAngle, 0);

            Gun.eulerAngles = desiredAngles;
            Gun.rotation = Quaternion.Lerp(lastRotation, Gun.rotation, RotationSmoothing);

            // if there is directional input we autofire
            IsShooting = true;
        }
        else
        {
            // if there is no directional we should NOT fire
            IsShooting = false;
        }
    }
}
