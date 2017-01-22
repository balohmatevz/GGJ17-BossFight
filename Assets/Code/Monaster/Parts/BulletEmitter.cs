using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public float SHOOT_INTERVAL = 4f;
    public float MAX_ANGLE = 45f;
    public int NUMBER_OF_SHOTS = 10;
    public const float BULLET_SPEED = 40;
    public float BULLET_MAX_DIST = 10;
	public float DELAY_BETWEEN_BULLETS = 0.1f;
	public float RADIUS = 0f;

    protected List<float> bulletRotations = new List<float>();
    public float ShootTimer;
    public float BulletTimer;
    public bool IsFriendly = false;
	public bool Shooting = false;

    public ParticleSystem BulletFire;
    public ParticleSystem BulletFlash;
    public GameObject BulletFireGO;
    public ParticleSystem MuzzleFlash;
    public GameObject MuzzleFlashGO;
    public GameObject BulletFlashGO;
    public Turret Turret;
    public CarBehaviour Car;

    // Use this for initialization
    protected virtual void Start()
    {
        ShootTimer = SHOOT_INTERVAL;
        BulletTimer = DELAY_BETWEEN_BULLETS;
        BulletFire.Stop();
        BulletFlash.Stop();
        MuzzleFlash.Stop();
        ShootTimer = SHOOT_INTERVAL;
        BulletTimer = DELAY_BETWEEN_BULLETS;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Turret != null && Turret.isDead)
        {
            return;
            //Dead turret
        }

        if(Car != null && !Car.IsShooting) {
            return;
            //Not currently shooting
        }

		if (!Shooting) {
        ShootTimer -= Time.deltaTime;
	        if (ShootTimer <= 0)
	        {
	            //ShootTimer += SHOOT_INTERVAL;
	            Shoot();
	        }
		} else {
	        BulletTimer -= Time.deltaTime;
	        if (BulletTimer <= 0)
	        {
	            BulletTimer += DELAY_BETWEEN_BULLETS;
	            if (bulletRotations.Count > 0)
	            {
	                SpawnBullet(bulletRotations[0]);
	                bulletRotations.RemoveAt(0);
					if (bulletRotations.Count == 0) {
						Shooting = false;
						ShootTimer = SHOOT_INTERVAL;
					}
	            }
	        }
		}
    }

    public void Shoot(float angleOffset = 0f)
    {
        float angleDiff = (2 * MAX_ANGLE) / NUMBER_OF_SHOTS;
        bulletRotations = new List<float>();

        for (int i = 0; i < NUMBER_OF_SHOTS; i++)
        {
            float angle = this.transform.rotation.eulerAngles.y;
            angle -= MAX_ANGLE + angleOffset;
            angle += i * angleDiff;
            bulletRotations.Add(angle);
        }
        BulletTimer = DELAY_BETWEEN_BULLETS;
		Shooting = true;
    }

    public void SpawnBullet(float rotation)
    {
        GameObject go;
        if (IsFriendly)
        {
            go = Instantiate(GameController.obj.PF_BulletFriendly);
        }
        else
        {
            go = Instantiate(GameController.obj.PF_BulletEnemy);
        }
        Transform t = go.transform;
        t.SetParent(GameController.obj.BulletAnchor);
        t.position = this.transform.position;
        Bullet bullet = go.GetComponent<Bullet>();
        BulletFireGO.transform.rotation = Quaternion.Euler(0, rotation, 0);
        BulletFlashGO.transform.rotation = Quaternion.Euler(0, rotation, 0);
        BulletFire.Emit(1);
		BulletFlash.Play();
		MuzzleFlash.Play();
		bullet.SetUp(this.transform.position, rotation, BULLET_SPEED, BULLET_MAX_DIST, RADIUS);
    }
}
