using System;

namespace Model
{
    public interface IObservable<T>
    {
        event Action<T> OnChange;
        T Value { get; }
    }
}