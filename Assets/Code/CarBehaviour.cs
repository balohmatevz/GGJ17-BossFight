using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    public int health = 100;
    public bool IsShooting = false;
    public float SHOOT_THRESHOLD = 0.05f;

    public void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Joystick X")) > SHOOT_THRESHOLD || Mathf.Abs(Input.GetAxis("Joystick Y")) > SHOOT_THRESHOLD)
        {
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }
}
