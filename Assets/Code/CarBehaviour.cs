using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    public int health = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                //It was a bullet!
                if (bullet.IsFriendly)
                {
                    //It's a friendly bullet! No biggie :D
                    //NOOP
                }
                else
                {
                    //Oh no! It's an ENEMY bullet :o
                    OnHit();
                }
            }
        }

    }

    public void OnHit()
    {
        health -= 1;
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

    }
}
