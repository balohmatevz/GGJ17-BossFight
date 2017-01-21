using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHitEffect : BulletHitEffect
{

    public CarBehaviour car;

    public override void OnHit(Bullet bullet)
    {
        base.OnHit(bullet);
        car.OnHit(1);
    }

    public override void OnHit(Rocket rocket)
    {
        base.OnHit(rocket);
        car.OnHit(10);
    }

}
