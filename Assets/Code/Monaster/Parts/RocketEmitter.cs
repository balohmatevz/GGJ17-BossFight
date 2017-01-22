using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEmitter : MonoBehaviour
{
    public float SHOOT_INTERVAL = 1.5f;
    public float MAX_DIST_TO_FIRE = 40f;

    public float ShootTimer;
    public Turret Turret;

    // Use this for initialization
    void Start()
    {
        ShootTimer = SHOOT_INTERVAL;
        GameController.obj.RocketEmitters.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.obj.RocketsInFlight > 0)
        {
            return;
        }
        if (!CanFire())
        {
            return;
            //Dead turret
        }

        ShootTimer -= Time.deltaTime;
        if (!IsInRange())
        {
            ShootTimer = SHOOT_INTERVAL;
        }
        if (ShootTimer <= 0)
        {
            ShootTimer += SHOOT_INTERVAL;
            if (IsInRange())
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        SpawnRocket(GameController.obj.TargetMarker.transform.position);
    }

    public void SpawnRocket(Vector3 target)
    {
        GameObject go = Instantiate(GameController.obj.PF_Rocket);
        Transform t = go.transform;
        t.SetParent(GameController.obj.BulletAnchor);
        t.position = this.transform.position;
        Rocket rocket = go.GetComponent<Rocket>();
        rocket.SetUp(this.transform.position, this.transform.forward, target);
        GameController.obj.RocketsInFlight++;
    }

    public bool CanFire()
    {
        return !Turret.isDead;
    }

    public bool IsInRange()
    {
        if (GameController.obj.car != null)
        {
            return Vector3.Distance(GameController.obj.car.transform.position, this.transform.position) <= MAX_DIST_TO_FIRE;
        }
        return false;
    }
}
