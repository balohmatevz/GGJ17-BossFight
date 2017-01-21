using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHitEffect : BulletHitEffect
{
    // Car has 100 HP
    const int BULLET_HIT = 20;
    const int BULLET_ROCKET = 60;

    public CarBehaviour car;

    public override void OnHit(Bullet bullet)
    {
        base.OnHit(bullet);
        car.OnHit(BULLET_HIT);
    }

    public override void OnHit(Rocket rocket)
    {
        base.OnHit(rocket);
        car.OnHit(BULLET_ROCKET);
    }

}
