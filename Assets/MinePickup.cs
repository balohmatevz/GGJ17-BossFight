using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePickup : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 90 * Time.deltaTime, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null)
        {

            if (collision.gameObject.GetComponent<CarIdentifier>() != null)
            {
                GameController.obj.MinesAcquired();
                Destroy(this.gameObject);
            }
        }
    }


}
