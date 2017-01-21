using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController obj;

    public enum GameStages
    {
        INIT, INTRODUCTION, STAGE_1, TRANSITION_TO_STAGE_2, STAGE_2
    }

    #region CONSTANTS

    public const int MONSTER_HEALTH = 100;

    #endregion CONSTANTS

    #region PUBLIC MEMBERS

    [Header("Variables")]
    public int health;
    public Vector2 LastMousePos;
    public int RocketsInFlight;
    public bool IsInRangeOfRocket = false;
    public List<Turret> Turrets = new List<Turret>();
    public GameStages GameStage = GameStages.INIT;

    [Header("Scene references")]
    public Camera cam;
    public Transform camT;
    public Transform BulletAnchor;
    public GameObject TargetMarker;
    public GameObject GroundImpact;
    public ParticleSystem GroundImpactPS;
    public CarBehaviour car;

    public List<RocketEmitter> RocketEmitters = new List<RocketEmitter>();
    public GameObject WorkStage1;
    public Animator WormStage1Animator;
    public float WormStage1Timer = 5f;

    [Header("Prefabs")]
    public GameObject PF_BulletFriendly;
    public GameObject PF_BulletEnemy;
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
        GameController.obj.GroundImpactPS.Stop();
        PlayIntroduction();
    }

    public void Frame()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 deltaMousePos = (Vector2)Input.mousePosition - (Vector2)LastMousePos;

                camT.Rotate(0, deltaMousePos.x * 0.4f, 0, Space.World);
                camT.Rotate(-deltaMousePos.y * 0.4f, 0, 0, Space.Self);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireShot(camT.position, camT.forward);
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
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
        }

        IsInRangeOfRocket = false;

        switch (GameStage)
        {
            case GameStages.INTRODUCTION:
                EndIntroduction();  //TODO
                break;
            case GameStages.STAGE_1:
                foreach (RocketEmitter rem in RocketEmitters)
                {
                    if (!rem.CanFire())
                    {
                        continue;
                    }
                    IsInRangeOfRocket |= rem.IsInRange();
                }

                if (Turrets.Count <= 3)
                {
                    TransitionToStage2();
                }
                break;
            case GameStages.TRANSITION_TO_STAGE_2:
                int i = 0;
                while (i < Turrets.Count)
                {
                    Turrets[i].OnDeath();
                }

                WormStage1Timer -= Time.deltaTime;
                if(WormStage1Timer < 0) {
                    WormStage1Animator.SetBool("Stage1Complete", true);
                }
                //TODO
                break;
            case GameStages.STAGE_2:
                //TODO
                break;
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

    public void PlayIntroduction()
    {
        //TODO
        GameStage = GameStages.INTRODUCTION;
        WorkStage1.SetActive(true);
    }

    public void EndIntroduction()
    {
        //TODO
        GameStage = GameStages.STAGE_1;
    }

    public void TransitionToStage2()
    {
        //TODO
        GameStage = GameStages.TRANSITION_TO_STAGE_2;
        WormStage1Timer = 5f;
    }
}
