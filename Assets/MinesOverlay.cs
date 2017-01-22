using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesOverlay : MonoBehaviour
{

    public float timer = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
