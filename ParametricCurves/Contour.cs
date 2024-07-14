using ParametricCurves.Extensions;
using System.Collections;

namespace ParametricCurves;

public class Contour : ISet<Curve>
{
    private HashSet<Curve> _curves { get; } = [];
    public IReadOnlySet<Curve> Curves => _curves;

    public int Count => _curves.Count;

    public bool IsReadOnly => false;

    public bool Add(Curve curve)
    {
        curve._contour ??= this;
        return curve._contour == this;
    }
    public static bool operator+(Contour contour, Curve curve) => contour.Add(curve);

    public void ExceptWith(IEnumerable<Curve> other)
    {
        foreach (var curve in other)
        {
            _curves.Remove(curve);
        }

        _curves.RemoveWhere(curve => other.Contains(curve));
    }

    public void IntersectWith(IEnumerable<Curve> other) => _curves.RemoveWhere(curve => !other.Contains(curve));
    public bool IsProperSubsetOf(IEnumerable<Curve> other)
        => other.Any(Contains) && this.All(curve => other.Contains(curve));
    public bool IsProperSupersetOf(IEnumerable<Curve> other)
        => this.Any(Contains) && other.All(Contains);
    public bool IsSubsetOf(IEnumerable<Curve> other) => _curves.All(curve => other.Contains(curve));
    public bool IsSupersetOf(IEnumerable<Curve> other) => other.All(Contains);
    public bool Overlaps(IEnumerable<Curve> other)
        => _curves.Any(curve => other.Contains(curve)) || other.Any(Contains);
    public bool SetEquals(IEnumerable<Curve> other) => IsSubsetOf(other) && (Count == other.Count());
    public void SymmetricExceptWith(IEnumerable<Curve> other) => throw new NotImplementedException();
    public void UnionWith(IEnumerable<Curve> other) => _curves.AddRange(other);
    void ICollection<Curve>.Add(Curve item) => throw new NotImplementedException();
    public void Clear() => RemoveAll();
    public bool Contains(Curve item) => _curves.Contains(item);
    public void CopyTo(Curve[] array, int arrayIndex) => throw new NotImplementedException();
    public bool Remove(Curve item)
    {
        if (item._contour == this) item._contour = null;
        return _curves.Remove(item);
    }
    public int RemoveWhere(Predicate<Curve> match)
    {
        int count = _curves.Count;
        this.Where(x => match(x)).ForEach(x => Remove(x));
        return _curves.Count - count;
    }
    public void RemoveAll() => _curves.ForEach(x => Remove(x));

    public IEnumerator<Curve> GetEnumerator() => _curves.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _curves.GetEnumerator();
    
}