using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{

    public int health = 100;
    public float SHOOT_THRESHOLD = 0.05f;

    private GunController gun;

    public bool IsShooting
    {
        get
        {
            return gun.IsShooting;
        }
    }

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
        GameController.obj.OnPlayerDeath();
    }

    // Use this for initialization
    void Start()
    {
        gun = GetComponent<GunController>();
    }
}
