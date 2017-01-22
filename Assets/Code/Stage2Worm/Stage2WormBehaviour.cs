using UnityEngine;

public class Stage2WormBehaviour : MonoBehaviour
{
    public WormBulletEmitter bulletEmitter;

    public void ShootBurst()
    {
        bulletEmitter.ShootBurst();
    }
}
