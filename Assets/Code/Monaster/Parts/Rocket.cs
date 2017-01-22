using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector3 origin;
    public Vector3 target;
    public Vector3 originTangent;
    public Vector3 targetTangent;
    public AnimationCurve InterpolationCurve;
    public const float TARGET_TANGENT_HEIGHT = 20f;
    public float SPEED = 1f;
    public Transform t;
    private float normalizeLifetime = 0f;
    public bool IsDisabled = true;

	public Renderer RocketRenderer;
	public Renderer RocketCubeRenderer;
	public Collider Collider;

    // Use this for initialization
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
		if (IsDisabled)
			return;
        normalizeLifetime += Time.deltaTime * SPEED;
        float curvedLifetime = InterpolationCurve.Evaluate(normalizeLifetime);
        this.transform.position = BezierUtil.GetPoint(origin, originTangent, targetTangent, target, curvedLifetime);
        this.transform.forward = BezierUtil.GetFirstDerivative(origin, originTangent, targetTangent, target, curvedLifetime);

        if (normalizeLifetime >= 1)
        {
            OnImpact();
        }
    }

    public void SetUp(Vector3 origin, Vector3 originForward, Vector3 target)
    {
        normalizeLifetime = 0;
        t = this.transform;
        t.position = origin;
        this.origin = origin;
        this.target = target;
        this.targetTangent = target + Vector3.up * TARGET_TANGENT_HEIGHT;
        this.originTangent = origin + originForward * TARGET_TANGENT_HEIGHT;
        IsDisabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is WheelCollider)
        {
            return;
        }

        if (!IsDisabled && other.gameObject != null)
        {
            OnHit();
            BulletHitEffect hitEffect = other.gameObject.GetComponent<BulletHitEffect>();
            if (hitEffect != null)
            {
                hitEffect.OnHit(this);
            }
        }
    }

    //Hits something other than the ground on the end.
    public void OnHit()
    {
        if (!IsDisabled)
        {
            IsDisabled = true;
            GameController.obj.RocketsInFlight--;
            GameController.obj.GroundImpact.transform.position = this.transform.position;
            GameController.obj.GroundImpactPS.Play();
            Destroy(this.gameObject);
        }
    }

    //Hits ground without hitting anything on hte way
    public void OnImpact()
    {
        GameController.obj.RocketsInFlight--;
        GameController.obj.GroundImpact.transform.position = this.transform.position;
        GameController.obj.GroundImpactPS.Play();
        Destroy(this.gameObject);
    }
}
