//#define DEV_MODE

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

    [Header("Prefabs")]
    public GameObject ProjectilePrefab;

    [Header("Variables")]
    public float RotationSmoothing = 0.1f;

    #endregion Public Members

    #region Private Members

    private float lastFireTime = 0f;
    private bool isFiring = false;
    
    #endregion Private Members
    
    void Update ()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;

        float h = Input.GetAxis("Joystick X");
        float v = Input.GetAxis("Joystick Y");

        var dir = new Vector3(h, 0f, v);

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
            isFiring = true;
        }
        else
        {
            // if there is no directional we should NOT fire
            isFiring = false;
        }

        if (isFiring)
        {
            // TODO: Fire projectiles
        }
        else
        {

        }

#if DEV_MODE
        if (GunParticleSystem != null)
        {
            if (isFiring)
            {
                if (!GunParticleSystem.isPlaying)
                {
                    GunParticleSystem.Play();
                }
            }
            else
            {
                GunParticleSystem.Stop();
            }
        }
#endif
    }
}
