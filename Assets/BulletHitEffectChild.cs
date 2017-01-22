using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitEffectChild : BulletHitEffect
{
    public WormStage2Hit wormStage2Hit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnHit(Bullet bullet)
    {
        Debug.Log("HIT");
        base.OnHit(bullet);
        if (bullet.IsFriendly)
        {
            if (GameController.obj.IsWormWounded)
            {
                wormStage2Hit.health -= 1;
            }
        }
    }
}
