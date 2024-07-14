using System.Windows;
using System.Collections.Generic;
using static System.Math;

namespace ParametricCurves.Extensions;

public static class VectorExtensions
{
    private static readonly double _sqrt2 = Sqrt(2);

    // Optional for more accuracy:
    private static readonly double _sqrt5 = Sqrt(5);

    public static double SquareLength(this Vector v) => Max(Abs(v.X), Abs(v.Y));
    private static double SquareDiamondLength(this Vector v) => _sqrt2 * (Abs(v.X) + Abs(v.Y));
    public static double OctagonLength(this Vector v) => Max(v.SquareLength(), v.SquareDiamondLength());

    // This is for additional accuracy.
    // May be faster to add by itself rather than multiply by 2.
    // But I will test that later, as I may not event bother with this.
    private static double XDiamondLength(this Vector v) => _sqrt5 * (2 * Abs(v.X) + Abs(v.Y));
    private static double YDiamondLength(this Vector v) => _sqrt5 * (Abs(v.X) + 2 * Abs(v.Y));
    public static double LengthApprox(this Vector v)
        => new double[4] { SquareLength(v), SquareDiamondLength(v), XDiamondLength(v), YDiamondLength(v) }.Max();
}
