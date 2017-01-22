using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {

        public static Vector2 Rotate(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public Vector2 Vector2FromAngle(float a)
        {
            a *= Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        }

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
            //Vector2 dir2D = new Vector2(h, v);
            //
            //
            //float dirAngle = Vector2.Angle(Vector2.right, dir2D);
            //float camAngle = Camera.main.transform.rotation.eulerAngles.y;
            //
            //if (dir2D.y < 0)
            //{
            //    dirAngle = -dirAngle;
            //}
            //
            //Vector2 dirGen = Vector2FromAngle(dirAngle + camAngle);
            //Vector3 camFW = Camera.main.transform.forward;
            //camFW.x = 0;
            //camFW.z = 0;
            //Vector3 dir = new Vector3(h, 0, v) + new Vector3(h + Mathf.Rad2Deg * Mathf.Cos(Mathf.Deg2Rad * camFW.y), 0, v + Mathf.Rad2Deg * Mathf.Sin(Mathf.Deg2Rad * camFW.y)).normalized;
            //
            //Debug.Log(dir);

            var d2 = Quaternion.Inverse(m_Car.transform.rotation) * dir;

            float brake = 0f;
            bool reverse = false;
            if (Input.GetButton("Fire2"))
            {
                //Debug.Log("Reverse");
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
