using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormStage2Hit : BulletHitEffect
{
    public static WormStage2Hit obj;
    public int health = 100;

    protected void Awake()
    {
        obj = this;
    }

    protected void Update()
    {
        if (health <= 0)
        {
            GameController.obj.Win();
            this.enabled = false;
        }
    }

    public override void OnHit(Bullet bullet)
    {
        Debug.Log("HIT2");
        base.OnHit(bullet);
        if (bullet.IsFriendly)
        {
            if (GameController.obj.IsWormWounded)
            {
                health -= 1;
            }
        }
    }
}
