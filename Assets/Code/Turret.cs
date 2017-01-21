using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Animator TurretAnimator;

    private bool isDead = false;
    private Collider[] colliders;

    // Use this for initialization
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
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
            Invoke("DisableColliders", 5f);
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
