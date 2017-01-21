using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Animator TurretAnimator;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDeath()
    {
        TurretAnimator.SetBool("Destroyed", true);
    }
}
