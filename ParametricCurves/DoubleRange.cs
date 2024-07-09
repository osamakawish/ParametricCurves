namespace ParametricCurves;

public readonly struct DoubleRange(double num1, double num2)
{
    public static DoubleRange Null { get; } = new DoubleRange(double.NaN, double.NaN);
    public bool IsNull { get; } = double.IsNaN(num1) || double.IsNaN(num2);

    public double Min { get; } = Math.Min(num1, num2);
    public double Max { get; } = Math.Max(num1, num2);

    public bool Contains(double value) => Min <= value && value <= Max;

    public static DoubleRange operator&(DoubleRange left, DoubleRange right)
    {
        double lo = Math.Max(left.Min, right.Min);
        double hi = Math.Min(left.Max, right.Max);

        return lo <= hi ? new DoubleRange(lo, hi) : Null;
    }

    public static DoubleRange operator |(DoubleRange left, DoubleRange right)
        => new(Math.Min(left.Min, right.Min),
               Math.Max(left.Max, right.Max));

    /// <summary>
    /// If the ranges intersect, then the intersecting range is returned. Otherwise, the range between the two ranges is returned.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static DoubleRange operator ^(DoubleRange left, DoubleRange right)
        => new(Math.Max(left.Min, right.Min), Math.Min(left.Max, right.Max));

    public static (DoubleRange left, DoubleRange right) operator -(DoubleRange left, DoubleRange right)
        => (new(left.Min, right.Min >= left.Min ? right.Min : double.NaN),
            new(right.Max <= left.Max ? right.Max : double.NaN, left.Max));


    public static implicit operator DoubleRange((double num1, double num2) num) => new(num.num1, num.num2);
    public void Deconstruct(out double num1, out double num2) => (num1, num2) = (Min, Max);
}
