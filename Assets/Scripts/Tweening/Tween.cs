using System;

interface Tween<T>
{
    bool Finished { get; }
    float Progress { get; }
    T Step(float t);
}
