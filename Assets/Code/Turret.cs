using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Animator TurretAnimator;

    private bool isDead = false;
    private Collider[] colliders;
    private RocketEmitter rocketEmitter;

    // Use this for initialization
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        rocketEmitter = GetComponentInChildren<RocketEmitter>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDeath()
    {
        TurretAnimator.SetBool("Destroyed", true);

        if (!isDead)
        {
            isDead = true;
            rocketEmitter.enabled = false;
            Invoke("DisableColliders", 6f);
        }
    }

    public void DisableColliders()
    {
        Debug.Log("Disabling colliders.");
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
