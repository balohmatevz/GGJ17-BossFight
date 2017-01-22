using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletEmitter : BulletEmitter
{
    public float BurstDelay = 0.2f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        Shooting = false;
    }

    public void ShootBurst()
    {
        Invoke("BurstShot1", BurstDelay);
        Invoke("BurstShot2", BurstDelay + 0.1f);
        Invoke("BurstShot3", BurstDelay + 0.2f);
    }

    private void BurstShot1()
    {
        Shoot(3f);
        SpawnBullets();
    }

    private void BurstShot2()
    {
        Shoot(0f);
        SpawnBullets();
    }

    private void BurstShot3()
    {
        Shoot(6f);
        SpawnBullets();
    }

    private void SpawnBullets()
    {
        if (Mathf.Abs(DELAY_BETWEEN_BULLETS) < 0.001f)
        {
            // If delay is 0 spawn all bullets at once
            for (int i = 0; i < bulletRotations.Count; i++)
            {
                SpawnBullet(bulletRotations[i]);
            }
            bulletRotations.Clear();
        }
        else
        {
            // Spawn one
            if (bulletRotations.Count > 0)
            {
                SpawnBullet(bulletRotations[0]);
                bulletRotations.RemoveAt(0);
            }
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        if (!Shooting)
        {
            return;
        }

        //ShootTimer -= Time.deltaTime;
        //if (ShootTimer <= 0)
        //{
        //    ShootTimer += SHOOT_INTERVAL;
        //    Shoot();
        //}

        //BulletTimer -= Time.deltaTime;
        //if (BulletTimer <= 0)
        //{
        //    BulletTimer += DELAY_BETWEEN_BULLETS;
        //    SpawnBullets();
        //}
    }
}
