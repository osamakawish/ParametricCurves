
using System.Collections.Immutable;
using System.Numerics;

namespace ParametricCurves;

public class Monotone(SortedSet<double> values, bool isForward = true)
{
    public static Monotone FromPartition(Partition partition, bool alongPartitionDirection = true)
        => new(partition.SortedValues, partition.IsForward == alongPartitionDirection);

    public Monotone(IEnumerable<double> values, bool isForward = true) : this(new SortedSet<double>(values), isForward) { }

    public static Monotone Null { get; } = new([double.NaN], true);
    public bool IsNull => this == Null;

    private SortedSet<double> _values { get; } = values;
    public IReadOnlySet<double> Values => _values;

    public bool IsBefore(double value) => isForward ? value <= Start : value >= Start;
    public bool IsAfter(double value) => isForward ? value >= End : value <= End;

    public double Start => isForward ? _values.Min : _values.Max;
    public double End => isForward ? _values.Max : _values.Min;

    public double Min => _values.Min;
    public double Max => _values.Max;
    public DoubleRange Range => new(Min, Max);

    public double Span => _values.Max - _values.Min;
    public int Count => _values.Count;

    public bool Prepend(double value) => IsBefore(value) && _values.Add(value);
    public bool Append(double value) => IsAfter(value) && _values.Add(value);

    private double Pop(double value) { _values.Remove(value); return value; }
    public double PopStart() => Pop(Start);
    public double PopEnd() => Pop(End);

    public bool Contains(double value) => _values.Min <= value && value <= _values.Max;
    public bool Intersects(Monotone other) => Contains(other.Min) || Contains(other.Max);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exists"></param>
    /// <param name="other"></param>
    /// <returns>If the intersection exists, this returns the smallest range common to both. If the intersection does not exist, 
    /// this outputs the range between the two Monotones.</returns>
    public bool TryIntersectBounds(out DoubleRange range, Monotone other)
        => (range = Range & other.Range).IsNull;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <param name="isForward"></param>
    /// <param name="exists"></param>
    /// <returns></returns>
    /// <remarks><b>Note.</b> This may cause additional issues.</remarks>
    public Monotone Intersect(Monotone other, bool isForward = true)
        => TryIntersectBounds(out var range, other)
            ? UnionEnumerators(isForward, EnumerateOverRange(this, range), EnumerateOverRange(other, range))
            : Null;

    public Monotone InRange(DoubleRange range, bool isForward = true) => new(Values.Where(range.Contains), isForward);

    public Monotone IntersectOverRange(Monotone other, DoubleRange range, bool isForward = true)
    {
        var monotone1 = InRange(range, isForward);
        var monotone2 = other.InRange(range, isForward);

        return monotone1.Intersect(monotone2, isForward);
    }

    private static IEnumerator<double> EnumerateOverRange(Monotone other, DoubleRange range)
    {
        var j = other.Values
            .Where(x => range.Min <= x && x <= range.Max)
            .GetEnumerator();
        return j;
    }

    public static Monotone operator &(Monotone left, Monotone right) => left.Intersect(right);

    public Monotone Union(Monotone other, bool isForward = true)
        => UnionEnumerators(isForward, Values.GetEnumerator(), other.Values.GetEnumerator());

    private static Monotone UnionEnumerators(bool isForward, IEnumerator<double> i, IEnumerator<double> j)
    {
        bool iterI = i.MoveNext();
        bool iterJ = j.MoveNext();

        List<double> values = [];

        while (iterI || iterJ)
        {
            if (!iterJ || i.Current < j.Current) { values.Add(i.Current); iterI = i.MoveNext(); }
            else { values.Add(j.Current); iterJ = j.MoveNext(); }
        }

        return new(values, isForward);
    }

    public static Monotone operator|(Monotone left, Monotone right) => left.Union(right);

    public (Monotone left, Monotone right) Subtract(Monotone other, bool isForward = true)
    {
        var (left, right) = Range - other.Range;
        return (IntersectOverRange(other, left, isForward), IntersectOverRange(other, right, isForward));
    }

    public static (Monotone left, Monotone right) operator -(Monotone left, Monotone right) => left.Subtract(right);

    public Monotone Add(Monotone other, bool isForward = true) => throw new NotImplementedException();

    public static Monotone operator +(Monotone left, Monotone right) => left.Add(right);
}
