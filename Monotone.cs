
using System.Collections;
using System.Collections.Immutable;
using System.Numerics;

namespace ParametricCurves;

public class Monotone(SortedSet<double> values, bool forward = true) : IEnumerable<double>
{
    public Monotone(bool forward = true) : this([], forward) { }

    public static Monotone FromPartition(Partition partition, bool alongPartitionDirection = true)
        => new(partition.SortedValues, partition.Forward == alongPartitionDirection);

    public Monotone(IEnumerable<double> values, bool forward = true) : this(new SortedSet<double>(values), forward) { }

    public static Monotone Null { get; } = new([double.NaN], true);
    public bool IsNull => this == Null;

    public IReadOnlySet<double> Values => values;

    public bool IsBefore(double value) => Forward ? value <= Start : value >= Start;
    public bool IsAfter(double value) => Forward ? value >= End : value <= End;

    public bool Forward { get => forward; set => forward = value; }
    public double Start => Forward ? values.Min : values.Max;
    public double End => Forward ? values.Max : values.Min;

    public double Min => values.Min;
    public double Max => values.Max;
    public DoubleRange Range => new(Min, Max);

    public double Span => Max - Min;
    public int Count => values.Count;

    public bool Prepend(double value) => IsBefore(value) && values.Add(value);
    public bool Append(double value) => IsAfter(value) && values.Add(value);

    private double Pop(double value) { values.Remove(value); return value; }
    public double PopStart() => Pop(Start);
    public double PopEnd() => Pop(End);

    public bool Contains(double value) => values.Min <= value && value <= values.Max;
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
    /// <param name="forward"></param>
    /// <param name="exists"></param>
    /// <returns></returns>
    /// <remarks><b>Note.</b> This may cause additional issues.</remarks>
    public Monotone Intersect(Monotone other, bool forward = true)
        => TryIntersectBounds(out var range, other)
            ? UnionEnumerators(forward, EnumerateOverRange(this, range), EnumerateOverRange(other, range))
            : Null;

    public Monotone InRange(DoubleRange range, bool forward = true) => new(Values.Where(range.Contains), forward);

    public Monotone IntersectOverRange(Monotone other, DoubleRange range, bool forward = true)
    {
        var monotone1 = InRange(range, forward);
        var monotone2 = other.InRange(range, forward);

        return monotone1.Intersect(monotone2, forward);
    }

    private static IEnumerator<double> EnumerateOverRange(Monotone other, DoubleRange range)
    {
        var j = other.Values
            .Where(x => range.Min <= x && x <= range.Max)
            .GetEnumerator();
        return j;
    }

    /// <summary>
    /// Intersects the two monotones. The returned monotone is always in the forward direction.<br/>
    /// Use <see cref="Intersect(Monotone, bool)"/> to control the direction.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Monotone operator &(Monotone left, Monotone right) => left.Intersect(right);

    public Monotone Union(Monotone other, bool forward = true)
        => UnionEnumerators(forward, Values.GetEnumerator(), other.Values.GetEnumerator());

    private static Monotone UnionEnumerators(bool forward, IEnumerator<double> i, IEnumerator<double> j)
    {
        bool iterI = i.MoveNext();
        bool iterJ = j.MoveNext();

        List<double> values = [];

        while (iterI || iterJ)
        {
            if (!iterJ || i.Current < j.Current) { values.Add(i.Current); iterI = i.MoveNext(); }
            else { values.Add(j.Current); iterJ = j.MoveNext(); }
        }

        return new(values, forward);
    }

    public static Monotone operator|(Monotone left, Monotone right) => left.Union(right);

    public (Monotone left, Monotone right) Subtract(Monotone other, bool forward = true)
    {
        var (left, right) = Range - other.Range;
        return (IntersectOverRange(other, left, forward), IntersectOverRange(other, right, forward));
    }

    public static (Monotone left, Monotone right) operator -(Monotone left, Monotone right) => left.Subtract(right);

    public Monotone Shifted(double shift) => new(Values.Select(x => x + shift), Forward);
    public Monotone Add(Monotone other, bool forward = true) => Union(other.Shifted(Max - other.Min), forward);
    public IEnumerator<double> GetEnumerator() => Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Values).GetEnumerator();

    public static Monotone operator +(Monotone left, Monotone right) => left.Add(right);
}
