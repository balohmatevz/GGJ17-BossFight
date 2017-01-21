using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Transform the input to reflect the fixed camera view
            var dir = new Vector3(h, 0f, v);
            var d2 = Quaternion.Inverse(m_Car.transform.rotation) * dir;

            float brake = 0f;
            bool reverse = false;
            if (Input.GetButton("Fire2"))
            {
                Debug.Log("Reverse");
                d2 = new Vector3(Mathf.Sign(d2.x) * d2.x, 0f, 0f);
                brake = -1.0f;
                reverse = true;
            }
            else
            {
                if (dir.magnitude > 0.5f && d2.z < -0.2f)
                {
                    //Debug.Log("Reverse!");
                    d2 = new Vector3(Mathf.Sign(d2.x), 0f, 1f);
                }
                else if (dir.magnitude < 0.1f)
                {
                    //Debug.Log("Braking!");
                    d2 = new Vector3(Mathf.Sign(d2.x) * d2.x, 0f, 0f);
                    brake = -1.0f;
                }
            }

            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(d2.x, d2.z, brake, handbrake, reverse);
        }
    }
}
