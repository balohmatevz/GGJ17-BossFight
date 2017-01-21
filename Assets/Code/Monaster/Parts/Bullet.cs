using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public const float HOLE_POSITION_DIFFERENCE = 0.3f;

    public Vector3 origin;
    public float maxDist;
    public float Speed;
    public bool IsDisabled = true;

    public Transform t;
    public Rigidbody rb;

    public ParticleSystem Death;
    public ParticleSystem Hole;
    public GameObject DeathGO;
    public GameObject HoleGO;
    public ParticleSystem Projectile;
    public GameObject ProjectileGO;

    // Use this for initialization
    void Start()
    {
        t = this.transform;
        Death.Stop();
        Hole.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, 0, Speed * Time.deltaTime, Space.Self);
        if (Vector3.Distance(this.transform.position, origin) >= maxDist)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetUp(Vector3 origin, float rotation, float speed, float maxDist)
    {
        t = this.transform;
        rb = this.GetComponent<Rigidbody>();
        t.position = origin;
        t.rotation = Quaternion.Euler(0, rotation, 0);
        t.localScale = Vector3.one;
        this.Speed = speed;
        this.origin = origin;
        this.maxDist = maxDist;
        IsDisabled = false;
        Death.transform.localRotation = Quaternion.identity;
        Hole.transform.localRotation = Quaternion.identity;
        Projectile.Emit(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDisabled && other.gameObject != null)
        {
            OnHit();
            BulletHitEffect hitEffect = other.gameObject.GetComponent<BulletHitEffect>();
            if (hitEffect != null)
            {
                hitEffect.OnHit();
            }
        }
    }

    public void OnHit()
    {
        if (!IsDisabled)
        {
            Death.transform.Translate(0, 0, -HOLE_POSITION_DIFFERENCE, Space.Self);
            Hole.transform.Translate(0, 0, -HOLE_POSITION_DIFFERENCE, Space.Self);
            Death.Emit(1);
            Hole.Emit(1);
            IsDisabled = true;
            Destroy(ProjectileGO);
        }
    }
}
