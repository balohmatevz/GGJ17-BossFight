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

    public enum Stage2Parts
    {
        START, MOVING, POP_OUT, OUT_HEALTHY, OUT_WOUNDED, RETRACTING
    }

    #region CONSTANTS

    public const int MONSTER_HEALTH = 100;
    public float STAGE_2_MIN_CIRCLE_DIST = 53.1f;
    public float STAGE_2_MAX_CIRCLE_DIST = 118.9f;
    public float STAGE_2_MOVE_TIME = 10f;

    #endregion CONSTANTS

    #region PUBLIC MEMBERS

    [Header("Variables")]
    public int health;
    public Vector2 LastMousePos;
    public int RocketsInFlight;
    public bool IsInRangeOfRocket = false;
    public List<Turret> Turrets = new List<Turret>();
    public GameStages GameStage = GameStages.INIT;
    public Stage2Parts CurrentStage2Part = Stage2Parts.START;
    public Vector3 NextWormStage2Position = Vector3.zero;
    public float Stage2MoveTimer;

    [Header("Scene references")]
    public Camera cam;
    public Transform camT;
    public Transform BulletAnchor;
    public GameObject TargetMarker;
    public GameObject GroundImpact;
    public ParticleSystem GroundImpactPS;
    public CarBehaviour car;
    public AngularTweenBehaviour Stage2DustCloudController;
    public GameObject Stage2MoveParticles;
    public ParticleSystem Stage2MoveParticlesPS;
    public GameObject Stage2Worm;
    public Animator Stage2WormAnim;

    public List<RocketEmitter> RocketEmitters = new List<RocketEmitter>();
    public GameObject WorkStage1;
    public Animator WormStage1Animator;
    public float WormStage1Timer = 5f;
    public float WormStage1Timer2 = 5f;
    public float WormStage2TimerHealthy = 2.13333f;
    public float WormStage2TimerWounded = 5f;
    public float WormStage2TimerRetracting = 5f;

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
        Stage2Worm.SetActive(false);
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
                if (WormStage1Timer < 0)
                {
                    WormStage1Animator.SetBool("Stage1Complete", true);
                    WormStage1Timer2 -= Time.deltaTime;
                }

                if (WormStage1Timer2 <= 0)
                {
                    CurrentStage2Part = Stage2Parts.START;
                    GameStage = GameStages.STAGE_2;
                    WorkStage1.SetActive(false);
                }
                break;
            case GameStages.STAGE_2:
                switch (CurrentStage2Part)
                {
                    case Stage2Parts.START:
                        //Choose next spawn position
                        Vector3 dirVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                        dirVector.Normalize();
                        NextWormStage2Position = dirVector * Random.Range(STAGE_2_MIN_CIRCLE_DIST, STAGE_2_MAX_CIRCLE_DIST);
                        //Stage2MoveParticles.transform.position = NextWormStage2Position;
                        //Stage2MoveParticlesPS.Play();
                        Stage2DustCloudController.StartTween(Stage2Worm.transform.position, NextWormStage2Position, STAGE_2_MOVE_TIME);
                        Stage2MoveTimer = STAGE_2_MOVE_TIME;
                        CurrentStage2Part = Stage2Parts.MOVING;
                        Stage2Worm.transform.position = NextWormStage2Position;
                        break;
                    case Stage2Parts.MOVING:
                        Stage2MoveTimer -= Time.deltaTime;
                        WormStage2TimerHealthy = 5f;
                        WormStage2TimerWounded = 5f;
                        WormStage2TimerRetracting = 1.8f;
                        Stage2WormAnim.Rebind();
                        Stage2WormAnim.SetBool("HasRecovered", false);
                        //if (Stage2MoveTimer <= 0)
                        if (Stage2DustCloudController.Finished)
                        {
                            CurrentStage2Part = Stage2Parts.POP_OUT;
                            //Stage2MoveParticlesPS.Stop();
                            Stage2Worm.SetActive(true);
                            if (Random.Range(0f, 1f) < 0.5f)
                            {
                                Stage2WormAnim.SetBool("Bombed", true); //TODO
                                CurrentStage2Part = Stage2Parts.OUT_WOUNDED;
                            }
                            else
                            {
                                Stage2WormAnim.SetBool("Bombed", false); //TODO
                                CurrentStage2Part = Stage2Parts.OUT_HEALTHY;
                            }
                        }
                        break;
                    case Stage2Parts.POP_OUT:

                        break;
                    case Stage2Parts.OUT_HEALTHY:
                        WormStage2TimerHealthy -= Time.deltaTime;
                        if (WormStage2TimerHealthy <= 0)
                        {
                            CurrentStage2Part = Stage2Parts.RETRACTING;
                            Stage2WormAnim.Stop();
                        }
                        break;
                    case Stage2Parts.OUT_WOUNDED:
                        WormStage2TimerWounded -= Time.deltaTime;
                        if (WormStage2TimerWounded <= 0)
                        {
                            Stage2WormAnim.SetBool("HasRecovered", true);
                            CurrentStage2Part = Stage2Parts.RETRACTING;
                        }
                        break;
                    case Stage2Parts.RETRACTING:
                        WormStage2TimerRetracting -= Time.deltaTime;
                        if (WormStage2TimerRetracting <= 0)
                        {
                            Stage2WormAnim.SetBool("HasRecovered", false);
                            Stage2WormAnim.Stop();
                            CurrentStage2Part = Stage2Parts.START;
                        }
                        break;
                }
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

    public void OnPlayerDeath()
    {
        cam.GetComponent<KillCamera>().enabled = true;
    }
}
