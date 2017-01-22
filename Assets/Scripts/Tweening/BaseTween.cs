public abstract class BaseTween<T> : Tween<T>
{
    protected T from;
    protected T to;
    protected float speed;

    public float Progress
    {
        get;
        protected set;
    }

    public bool Finished
    {
        get
        {
            return Progress >= 1f;
        }
    }

    public BaseTween(T from, T to, float duration)
    {
        this.from = from;
        this.to = to;
        speed = 1f / duration;
        Progress = 0f;
    }

    public T Step(float t)
    {
        Progress += t * speed;
        return Lerp(Progress);
    }

    protected abstract T Lerp(float t);
}
