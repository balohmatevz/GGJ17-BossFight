using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    public int health = 100;

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

    }
}
