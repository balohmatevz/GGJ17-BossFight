using UnityEngine;

public class Stage2WormBehaviour : MonoBehaviour
{
    private WormBulletEmitter bulletEmitter;

    void Start()
    {
        bulletEmitter = GetComponentInChildren<WormBulletEmitter>();
    }

    public void ShootBurst()
    {
        bulletEmitter.ShootBurst();
    }
}
