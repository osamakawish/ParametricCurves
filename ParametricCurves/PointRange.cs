using ParametricCurves.Extensions;
using System.Windows;

namespace ParametricCurves;

public record PointRange(Point Start, Point End)
{
    public static PointRange FromXYRanges(DoubleRange xRange, DoubleRange yRange)
        => new(new(xRange.Min, yRange.Min), new(xRange.Max, yRange.Max));

    public double Left { get; } = Math.Min(Start.X, End.X);
    public double Right { get; } = Math.Max(Start.X, End.X);
    public double Bottom { get; } = Math.Min(Start.X, End.X);
    public double Top { get; } = Math.Max(Start.X, End.X);

    public DoubleRange XRange => new(Left, Right);
    public DoubleRange YRange => new(Bottom, Top);

    public Vector Vector { get; } = End - Start;

    public static Point NaNPoint { get; } = new(double.NaN, double.NaN);
    public bool IsNaN => Start.IsNaNPoint() || End.IsNaNPoint();
    public static PointRange NaN { get; } = new(NaNPoint, NaNPoint);

    public Rect AsRect => (Rect)this;

    public static implicit operator PointRange((Point point1, Point point2) points)
        => new(points.point1, points.point2);

    public static implicit operator Rect(PointRange pointRange) => pointRange.IsNaN ?
        Rect.Empty : new(pointRange.Start, pointRange.End);

    /// <summary>
    /// Flips the <see cref="PointRange"/> about <see cref="Start"/> or <paramref name="P"/>.
    /// </summary>
    /// <param name="P"></param>
    /// <returns></returns>
    public static PointRange operator -(PointRange P) => new(P.Start, P.Start - P.Vector);
    /// <summary>
    /// Takes the start of range <paramref name="A"/> and connects it to the end of range <paramref name="B"/>.
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public static PointRange operator |(PointRange A, PointRange B) => new(A.Start, B.End);
    /// <summary>
    /// Takes the end of range <paramref name="A"/> and connects it to the start of range <paramref name="B"/>.
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public static PointRange operator ^(PointRange A, PointRange B) => new(A.End, B.Start);
    public static PointRange operator +(PointRange A, PointRange B) => new(A.Start, A.End + B.Vector);

    // May be better to implement a VectorRange that subtracts the start and end points.
    public static VectorRange operator -(PointRange A, PointRange B) => new(A.Start - B.Start, A.End - B.End);

    /// <summary>
    /// Cases:
    /// <list type="bullet">
    /// <item><b>Either of the <see cref="PointRange"/>s are NaN or parallel.</b> In this case,
    /// <paramref name="pointOfIntersection"/> is null and false is returned. </item>
    /// <item><b>The <see cref="PointRange"/>s intersect, but outside their range.</b> Here,
    /// <paramref name="pointOfIntersection"/> is the intersection point, but false
    /// is returned.</item>
    /// <item><b>A POI exists and is within both ranges.</b> Here, <paramref name="pointOfIntersection"/>
    /// is the intersection point, and true is returned.</item>
    /// </list>
    /// </summary>
    /// <param name="B"></param>
    /// <param name="pointOfIntersection"></param>
    /// <returns>True if the POI (Point of Intersection) exists and is within both
    /// <see cref="PointRange"/>s. False otherwise.</returns>
    public bool Intersect(PointRange B, out Point? pointOfIntersection)
    {
        pointOfIntersection = null;
        if (IsNaN || B.IsNaN) return false;

        var AB = this - B;

        double x1 = Start.X, y1 = Start.Y;
        double x2 = End.X, y2 = End.Y;
        double x3 = B.Start.X, y3 = B.Start.Y;
        double x4 = B.End.X, y4 = B.End.Y;

        double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        double reciprocal = 1 / denominator;

        // Lines are parallel or coincident
        if (denominator == 0) return false;

        double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) * reciprocal;
        double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) * reciprocal;

        double intersectX = x1 + t * (x2 - x1);
        double intersectY = y1 + t * (y2 - y1);
        pointOfIntersection = new Point(intersectX, intersectY);

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

    public static Point? operator &(PointRange left, PointRange right)
        => left.Intersect(right, out var point) ? null : point;
}