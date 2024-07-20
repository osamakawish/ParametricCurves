using System.Windows;
using System.Collections.Generic;
using static System.Math;

namespace ParametricCurves.Extensions;

public static class VectorExtensions
{
    private static readonly double _sqrt2 = Sqrt(2);
    public static readonly double Poly8Tolerance = _sqrt2 - 1;

    // Optional for more accuracy:
    private static readonly double _sqrt5 = Sqrt(5);

    public static double SquareLength(this Vector v) => Max(Abs(v.X), Abs(v.Y));
    private static double SquareDiamondLength(this Vector v) => _sqrt2 * (Abs(v.X) + Abs(v.Y));
    /// <summary>
    /// Uses an octagonal approximation of a circle to estimate the length.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double Poly8Length(this Vector v) => Max(v.SquareLength(), v.SquareDiamondLength());

    // This is for additional accuracy.
    // May be faster to add by itself rather than multiply by 2.
    // But I will test that later, as I may not event bother with this.
    private static double XDiamondLength(this Vector v) => _sqrt5 * (2 * Abs(v.X) + Abs(v.Y));
    private static double YDiamondLength(this Vector v) => _sqrt5 * (Abs(v.X) + 2 * Abs(v.Y));
    /// <summary>
    /// Uses a 16-sided polygon to approximate the length.
    /// </summary>
    /// <remarks>Slower, but more accurate length approximation than <see cref="Poly8Length(Vector)"/>.</remarks>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double Poly16Length(this Vector v)
        => new double[4] { SquareLength(v), SquareDiamondLength(v), XDiamondLength(v), YDiamondLength(v) }.Max();

    public static double XDiamondLengthInner(this Vector v, double size)
        => ReciprocalEstimate(size) * (size * Abs(v.X) + Abs(v.Y));

    public static double YDiamondLengthInner(this Vector v, double size)
        => ReciprocalEstimate(size) * (size * Abs(v.X) + Abs(v.Y));

    public static double XDiamondLengthOuter(this Vector v, double size)
        => ReciprocalSqrtEstimate(size * size + 1) * (size * Abs(v.X) + Abs(v.Y));

    public static double YDiamondLengthOuter(this Vector v, double size)
        => ReciprocalSqrtEstimate(size * size + 1) * (size * Abs(v.X) + Abs(v.Y));
}
