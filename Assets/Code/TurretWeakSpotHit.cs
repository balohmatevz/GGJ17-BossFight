using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeakSpotHit : BulletHitEffect
{
    public TurretWeakSpot WeakSpot;

    public override void OnHit(Bullet bullet)
    {
        base.OnHit(bullet);
        WeakSpot.OnHit();
    }
}
