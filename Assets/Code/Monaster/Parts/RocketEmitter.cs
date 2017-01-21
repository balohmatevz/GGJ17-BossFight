using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEmitter : MonoBehaviour
{
    public const float SHOOT_INTERVAL = 1.5f;
    public const float BULLET_FORCE = 100;
    public const float BULLET_MAX_DIST = 10;

    public float ShootTimer = SHOOT_INTERVAL;

    // Use this for initialization
    void Start()
    {
        ShootTimer = SHOOT_INTERVAL;
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
}
