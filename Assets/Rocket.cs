using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector3 origin;
    public Vector3 target;

    public const float DISTANCE_THRESHOLD = 0.1f;
    public const float SPEED = 4f;
    public const float ARK_HEIGHT = 4f;
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
        Vector3 pos = Vector3.MoveTowards(this.transform.position, target, SPEED * Time.deltaTime);
        Vector2 startPos = new Vector2(origin.x, origin.z);
        Vector2 thisPos = new Vector2(pos.x, pos.z);
        Vector2 endPos = new Vector2(target.x, target.z);
        float pathPercentage = Vector2.Distance(startPos, thisPos) / Vector2.Distance(startPos, endPos);
        float posY = Mathf.Sin(pathPercentage * Mathf.PI) * ARK_HEIGHT;
        pos.y = posY;
        this.transform.position = pos;
        if (Vector3.Distance(this.transform.position, target) < DISTANCE_THRESHOLD)
        {
            GameController.obj.RocketsInFlight--;
            Destroy(this.gameObject);
        }
    }

    public void SetUp(Vector3 origin, Vector3 target)
    {
        t = this.transform;
        rb = this.GetComponent<Rigidbody>();
        t.position = origin;
        this.origin = origin;
        this.target = target;
    }
}
