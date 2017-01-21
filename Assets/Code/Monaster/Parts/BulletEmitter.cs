using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public const float SHOOT_INTERVAL = 4f;
    public const float MAX_ANGLE = 45f;
    public const int NUMBER_OF_SHOTS = 10;
    public const float BULLET_FORCE = 500;
    public const float BULLET_MAX_DIST = 10;
    public const float DELAY_BETWEEN_BULLETS = 0.1f;

    List<float> bulletRotations = new List<float>();
    public float ShootTimer = SHOOT_INTERVAL;
    public float BulletTimer = DELAY_BETWEEN_BULLETS;

    // Use this for initialization
    void Start()
    {
        ShootTimer = SHOOT_INTERVAL;
        BulletTimer = DELAY_BETWEEN_BULLETS;
    }

    // Update is called once per frame
    void Update()
    {
        ShootTimer -= Time.deltaTime;
        if (ShootTimer <= 0)
        {
            ShootTimer += SHOOT_INTERVAL;
            Shoot();
        }

        BulletTimer -= Time.deltaTime;
        if (BulletTimer <= 0)
        {
            BulletTimer += DELAY_BETWEEN_BULLETS;
            if (bulletRotations.Count > 0)
            {
                SpawnBullet(bulletRotations[0]);
                bulletRotations.RemoveAt(0);
            }
        }
    }

    public void Shoot()
    {
        float angleDiff = (2 * MAX_ANGLE) / NUMBER_OF_SHOTS;
        bulletRotations = new List<float>();

        for (int i = 0; i < NUMBER_OF_SHOTS; i++)
        {
            float angle = this.transform.rotation.eulerAngles.y;
            angle -= MAX_ANGLE;
            angle += i * angleDiff;
            bulletRotations.Add(angle);
        }
        BulletTimer = DELAY_BETWEEN_BULLETS;
    }

    public void SpawnBullet(float rotation)
    {
        GameObject go = Instantiate(GameController.obj.PF_Bullet);
        Transform t = go.transform;
        t.SetParent(GameController.obj.BulletAnchor);
        t.position = this.transform.position;
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.SetUp(this.transform.position, rotation, BULLET_FORCE, BULLET_MAX_DIST);
    }
}
