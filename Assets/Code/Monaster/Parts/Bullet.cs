using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 origin;
    public float maxDist;

    public Transform t;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, origin) >= maxDist)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetUp(Vector3 origin, float rotation, float force, float maxDist)
    {
        t = this.transform;
        rb = this.GetComponent<Rigidbody>();
        t.position = origin;
        t.rotation = Quaternion.Euler(0, rotation, 0);
        rb.AddForce(t.forward * force);
        this.origin = origin;
        this.maxDist = maxDist;
    }
}
