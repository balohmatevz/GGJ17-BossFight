using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionCamera : MonoBehaviour
{
    public float ROTATION_SPEED = 0.2f;
    public float CAM_DISTANCE = 150;

    public float timer = 0;
    public Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        timer = Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * ROTATION_SPEED;
        cam.transform.position = new Vector3(Mathf.Cos(timer) * CAM_DISTANCE, 40, Mathf.Sin(timer) * CAM_DISTANCE);
        cam.transform.LookAt(Vector3.zero);
    }
}
