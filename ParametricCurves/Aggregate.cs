
using System.Collections;
using System.Numerics;

namespace ParametricCurves;

public abstract class Aggregate<T> : ICollection<T>
    where T : INumber<T>
{
    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public abstract T Find(Func<T, bool> predicate);
    public T Find(T value) => Find(x => x == value);
    public abstract bool Contains(T value);
    public void Add(T item) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
    public bool Remove(T item) => throw new NotImplementedException();
    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    // Union

    // Intersect
}
