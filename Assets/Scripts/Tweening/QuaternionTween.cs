using UnityEngine;

public class QuaternionTween : BaseTween<Quaternion>
{
    public QuaternionTween(Quaternion from, Quaternion to, float duration) : base(from, to, duration) { }

    protected override Quaternion Lerp(float t)
    {
        return Quaternion.Lerp(from, to, t);
    }

    public static QuaternionTween AroundYAxis(Vector3 from, Vector3 to, float duration) {
        return new QuaternionTween(
            Quaternion.LookRotation(new Vector3(from.x, 0f, from.z)),
            Quaternion.LookRotation(new Vector3(to.x, 0f, to.z)),
            duration);
    }
}
