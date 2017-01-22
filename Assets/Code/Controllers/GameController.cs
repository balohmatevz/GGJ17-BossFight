using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController obj;
    public bool CanLayMines = false;
    public GameObject MinesOverlay;
    public GameObject PF_Mine;
    public float MineTimer = 5f;
    public List<Mine> Mines = new List<Mine>();
    public bool IsWormWounded = false;

    public GameObject Stage2Overlay;

    public float MINE_KILLS_WORM_THRESHOLD = 10f;

    public enum GameStages
    {
        INIT, INTRODUCTION, STAGE_1, TRANSITION_TO_STAGE_2, STAGE_2, DEAD, WIN
    }

    public enum Stage2Parts
    {
        START, MOVING, POP_OUT, OUT_HEALTHY, OUT_WOUNDED, RETRACTING
    }

    #region CONSTANTS

    public const int MONSTER_HEALTH = 100;
    public float STAGE_2_MIN_CIRCLE_DIST = 53.1f;
    public float STAGE_2_MAX_CIRCLE_DIST = 110.9f;
    public float STAGE_2_MOVE_TIME = 10f;
    public const float STAGE_2_POP_OUT_WINDUP_TIME = 4f;
    public const float STAGE_2_NEW_POSITION_MIN_ANGLE_DEG = 80f;
    public const float STAGE_2_NEW_POSITION_MAX_ANGLE_DEG = 160f;

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
    public float Stage2PopOutWindupTimeMoveTimer;

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
    public GameObject Introduction;
    public GameObject GameOver;
    public WormStage2Hit Wurm2Hit;
    public GameObject Victory;

    public List<RocketEmitter> RocketEmitters = new List<RocketEmitter>();
    public GameObject WorkStage1;
    public Animator WormStage1Animator;
    public Stage2WormBehaviour Stage2WormBehaviour;
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

    #region PRIVATE MEMBERS

    private float lastDirectionAngle = 180f;

    #endregion PRIVATE MEMBERS

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
        Stage2Worm.SetActive(false);
        Introduction.SetActive(false);
        GameOver.SetActive(false);
        Stage2Overlay.SetActive(false);
        Victory.SetActive(false);

        MineTimer = 0;
        MinesOverlay.SetActive(false);
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
            if (Input.GetKey(KeyCode.Alpha2))
            {
                TransitionToStage2();
            }
        }

        IsInRangeOfRocket = false;

        switch (GameStage)
        {
            case GameStages.INTRODUCTION:
                if (Input.GetButtonDown("B Button"))
                {
                    EndIntroduction();
                }
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


                Stage2Overlay.SetActive(true);

                if (CanLayMines)
                {
                    MineTimer -= Time.deltaTime;
                    if (Input.GetButtonDown("B Button"))
                    {
                        PlaceMine();
                    }
                }

                switch (CurrentStage2Part)
                {
                    case Stage2Parts.START:
                        //Choose next spawn position
                        lastDirectionAngle += Mathf.Sign(Random.value - 0.5f) * Random.Range(STAGE_2_NEW_POSITION_MIN_ANGLE_DEG, STAGE_2_NEW_POSITION_MAX_ANGLE_DEG);
                        lastDirectionAngle %= 360;

                        IsWormWounded = false;

                        var angleRad = lastDirectionAngle * Mathf.Deg2Rad;
                        Vector3 dirVector = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad));
                        NextWormStage2Position = dirVector * Random.Range(STAGE_2_MIN_CIRCLE_DIST, STAGE_2_MAX_CIRCLE_DIST);
                        //Stage2MoveParticles.transform.position = NextWormStage2Position;
                        //Stage2MoveParticlesPS.Play();
                        Stage2DustCloudController.StartTween(Stage2Worm.transform.position, NextWormStage2Position, STAGE_2_MOVE_TIME);
                        CurrentStage2Part = Stage2Parts.MOVING;
                        Stage2PopOutWindupTimeMoveTimer = STAGE_2_POP_OUT_WINDUP_TIME;
                        Stage2Worm.transform.position = NextWormStage2Position;
                        break;
                    case Stage2Parts.MOVING:
                        WormStage2TimerHealthy = 5f;
                        WormStage2TimerWounded = 5f;
                        WormStage2TimerRetracting = 1.8f;
                        Stage2WormAnim.Rebind();
                        IsWormWounded = false;
                        Stage2WormAnim.SetBool("HasRecovered", false);
                        //if (Stage2MoveTimer <= 0)
                        if (Stage2DustCloudController.Finished)
                        {
                            Stage2PopOutWindupTimeMoveTimer -= Time.deltaTime;
                            if (Stage2PopOutWindupTimeMoveTimer <= 0)
                            {
                                CurrentStage2Part = Stage2Parts.OUT_HEALTHY;
                                //Stage2MoveParticlesPS.Stop();
                                Stage2Worm.SetActive(true);
                                Stage2WormBehaviour.ShootBurst();

                                Vector2 pos2D = new Vector2(NextWormStage2Position.x, NextWormStage2Position.z);
                                foreach (Mine mine in Mines)
                                {
                                    Vector2 mine2D = new Vector2(mine.transform.position.x, mine.transform.position.z);
                                    Debug.Log(Vector2.Distance(pos2D, mine2D));
                                    if (Vector2.Distance(pos2D, mine2D) <= MINE_KILLS_WORM_THRESHOLD)
                                    {
                                        Stage2WormAnim.SetBool("Bombed", true); //TODO
                                        CurrentStage2Part = Stage2Parts.OUT_WOUNDED;
                                        break;
                                    }
                                    else
                                    {
                                        Stage2WormAnim.SetBool("Bombed", false); //TODO
                                        CurrentStage2Part = Stage2Parts.OUT_HEALTHY;
                                    }
                                }
                            }
                        }
                        break;
                    case Stage2Parts.POP_OUT:

                        break;
                    case Stage2Parts.OUT_HEALTHY:
                        IsWormWounded = false;
                        WormStage2TimerHealthy -= Time.deltaTime;
                        if (WormStage2TimerHealthy <= 0)
                        {
                            CurrentStage2Part = Stage2Parts.RETRACTING;
                            Stage2WormAnim.Stop();
                        }
                        break;
                    case Stage2Parts.OUT_WOUNDED:
                        IsWormWounded = true;
                        WormStage2TimerWounded -= Time.deltaTime;
                        if (WormStage2TimerWounded <= 0)
                        {
                            Stage2WormAnim.SetBool("HasRecovered", true);
                            CurrentStage2Part = Stage2Parts.RETRACTING;
                        }
                        break;
                    case Stage2Parts.RETRACTING:
                        IsWormWounded = false;
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
            case GameStages.DEAD:
                GameOver.SetActive(true);
                if (Input.GetButtonDown("B Button"))
                {
                    SceneManager.LoadScene("Game");
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
        GameStage = GameStages.INTRODUCTION;
        Introduction.SetActive(true);
        WorkStage1.SetActive(true);
        cam.GetComponent<KillCamera>().enabled = false;
        cam.GetComponent<FollowTarget>().enabled = false;
        cam.GetComponent<IntroductionCamera>().enabled = true;
    }

    public void EndIntroduction()
    {
        Introduction.SetActive(false);
        GameStage = GameStages.STAGE_1;
        cam.GetComponent<KillCamera>().enabled = false;
        cam.GetComponent<FollowTarget>().enabled = true;
        cam.GetComponent<IntroductionCamera>().enabled = false;
    }

    public void TransitionToStage2()
    {
        //TODO
        GameStage = GameStages.TRANSITION_TO_STAGE_2;
        WormStage1Timer = 5f;
    }

    public void OnPlayerDeath()
    {
        GameStage = GameStages.DEAD;
        cam.GetComponent<KillCamera>().enabled = true;
    }

    public void MinesAcquired()
    {
        CanLayMines = true;
        MinesOverlay.SetActive(true);
    }

    public void PlaceMine()
    {
        if (MineTimer > 0)
        {
            return;
        }

        MineTimer = 5f;
        GameObject go = Instantiate(PF_Mine);
        Transform t = go.transform;
        t.SetParent(BulletAnchor);
        t.position = car.transform.position;
        Mine mine = go.GetComponent<Mine>();
        Mines.Add(mine);
    }

    public void Win()
    {
        Victory.SetActive(true);
        GameStage = GameStages.WIN;
        Stage2Worm.SetActive(false);
    }
}
