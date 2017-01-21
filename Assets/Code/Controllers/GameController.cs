using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController obj;

    #region CONSTANTS

    public const int MONSTER_HEALTH = 100;

    #endregion CONSTANTS

    #region PUBLIC MEMBERS

    [Header("Variables")]
    public int health;
    public Vector2 LastMousePos;
    public int RocketsInFlight;

    [Header("Scene references")]
    public Camera cam;
    public Transform camT;
    public Transform BulletAnchor;
    public GameObject TargetMarker;

    [Header("Prefabs")]
    public GameObject PF_Bullet;
    public GameObject PF_Rocket;

    #endregion PUBLIC MEMBERS

    #region UNITY METHODS
    // Use this for initialization
    private void Awake()
    {
        Bootstrap();
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Frame();
    }
    #endregion UNITY METHODS

    public void Bootstrap()
    {
        obj = this;
    }

    public void Init()
    {
        health = MONSTER_HEALTH;
        LastMousePos = Input.mousePosition;
        RocketsInFlight = 0;
    }

    public void Frame()
    {
        Vector2 deltaMousePos = (Vector2)Input.mousePosition - (Vector2)LastMousePos;

        camT.Rotate(0, deltaMousePos.x * 0.4f, 0, Space.World);
        camT.Rotate(-deltaMousePos.y * 0.4f, 0, 0, Space.Self);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireShot(camT.position, camT.forward);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //UP
            camT.Translate(0, 10 * Time.deltaTime, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //DOWN
            camT.Translate(0, -10 * Time.deltaTime, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //FORWARD
            camT.Translate(0, 0, 10 * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //BACK
            camT.Translate(0, 0, -10 * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //LEFT
            camT.Translate(-10 * Time.deltaTime, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //RIGHT
            camT.Translate(10 * Time.deltaTime, 0, 0, Space.Self);
        }

        LastMousePos = Input.mousePosition;
    }

    //Fires a shot from origin in the spuuplied direction
    public void FireShot(Vector3 origin, Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit);
        if (hit.collider && hit.collider.gameObject)
        {
            MonsterPart monsterPart = hit.collider.gameObject.GetComponent<MonsterPart>();
            if (monsterPart != null)
            {
                if (monsterPart.CanBeHit)
                {
                    monsterPart.OnHit();
                }
            }
        }
    }
}
