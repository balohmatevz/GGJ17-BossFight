using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Animator TurretAnimator;

    public bool isDead = false;
    private Collider[] colliders;
	public Animation GunAnimation;
    private RocketEmitter rocketEmitter;

    // Use this for initialization
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        rocketEmitter = GetComponentInChildren<RocketEmitter>();
        GameController.obj.Turrets.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDeath()
    {
        TurretAnimator.SetBool("Destroyed", true);

        if (GameController.obj.Turrets.Contains(this))
        {
            GameController.obj.Turrets.Remove(this);
        }

        if (!isDead)
        {
            isDead = true;
			rocketEmitter.enabled = false;
			GunAnimation.Stop();
			GunAnimation.enabled=false;
            Invoke("DisableColliders", 6f);
        }
    }

    public void DisableColliders()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
