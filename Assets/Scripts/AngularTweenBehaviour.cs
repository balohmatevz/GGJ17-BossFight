using UnityEngine;

public class AngularTweenBehaviour : MonoBehaviour
{
    public Transform Target;
    //public Transform From;
    //public Transform To;

    public float Duration = 5f;

    private QuaternionTween rotationTween;
    private Vector3Tween positionTween;
    private FloatTween distanceTween;

    public bool Finished { get; private set; }

    public void Awake()
    {
        Finished = true;
    }

    public void Start()
    {
        //StartTween(From, To, Duration);
    }

    public void StartTween(Vector3 from, Vector3 to, float duration)
    {
        Target.SetParent(null);

        rotationTween = QuaternionTween.AroundYAxis(from - transform.position, to - transform.position, duration);

        transform.rotation = rotationTween.Step(0f);
        Target.SetParent(transform);

        distanceTween = new FloatTween((from - transform.position).magnitude, (to - transform.position).magnitude, duration);
        Target.position = transform.forward * distanceTween.Step(0f);

        Finished = false;
    }

    public void Update()
    {
        if (Finished) return;

        float deltaTime = Time.deltaTime;

        if (!rotationTween.Finished)
        {
            transform.rotation = rotationTween.Step(deltaTime);
        }

        if (!distanceTween.Finished)
        {
            Target.position = transform.forward * distanceTween.Step(deltaTime);
        }

        Finished = rotationTween.Finished && distanceTween.Finished;
    }
}
