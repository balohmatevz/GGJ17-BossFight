using UnityEngine;

public class FloatTween : BaseTween<float>
{
    public FloatTween(float from, float to, float duration) : base(from, to, duration) { }

    protected override float Lerp(float t)
    {
        return Mathf.Lerp(from, to, t);
    }
}
