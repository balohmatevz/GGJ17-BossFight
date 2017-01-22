using UnityEngine;

public class Vector3Tween : BaseTween<Vector3>
{
    public Vector3Tween(Vector3 from, Vector3 to, float duration) : base(from, to, duration) { }

    protected override Vector3 Lerp(float t)
    {
        return Vector3.Lerp(from, to, t);
    }
}
