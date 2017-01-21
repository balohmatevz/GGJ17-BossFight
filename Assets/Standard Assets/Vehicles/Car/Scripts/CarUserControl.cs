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

            //Debug.DrawLine(m_Car.transform.position, m_Car.transform.position + dir * 2f, Color.red);
            //Debug.DrawLine(m_Car.transform.position, m_Car.transform.position + d2 * 2f, Color.magenta);

#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(d2.x, d2.z, d2.z, handbrake);
#else
            m_Car.Move(d2.x, d2.z, d2.z, 0f);
#endif
        }
    }
}
