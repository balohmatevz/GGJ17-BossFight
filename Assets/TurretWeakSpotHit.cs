using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeakSpotHit : BulletHitEffect
{
    public TurretWeakSpot WeakSpot;

    public override void OnHit()
    {
        base.OnHit();
        WeakSpot.OnHit();
    }
}
