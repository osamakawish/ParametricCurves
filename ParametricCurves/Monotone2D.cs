using ParametricCurves.Extensions;
using System.Collections;
using System.Windows;

namespace ParametricCurves;

public class Monotone2D : IEnumerable<Point>
{
    private readonly Monotone _xMonotone;
    private readonly Monotone _yMonotone;
    public IEnumerable<Point> Points => _xMonotone.Zip(_yMonotone).Select(x => new Point(x.First, x.Second));
    public PointRange Range => new(new(_xMonotone.Start, _yMonotone.Start), new(_xMonotone.End, _yMonotone.End));

    public (bool xForward, bool yForward) Orientation
    {
        get => (_xMonotone.Forward,  _yMonotone.Forward);
        set => (_xMonotone.Forward, _yMonotone.Forward) = value;
    }

    public void FlipXOrientation() => _xMonotone.Forward = !_xMonotone.Forward;
    public void FlipYOrientation() => _yMonotone.Forward = !_yMonotone.Forward;

    public bool IsClockwise => Orientation.xForward ^ Orientation.yForward;
    public bool IsCounterClockwise => Orientation.xForward == Orientation.yForward;

    public Monotone2D(Monotone xMonotone, Monotone yMonotone)
    {
        if (xMonotone.Count != yMonotone.Count)
            throw new ArgumentException(
                "The two monotones must have the same number of elements. " +
                $"Specifically, {nameof(xMonotone)}.Count must equal {nameof(yMonotone)}.Count.");

        _xMonotone = xMonotone; _yMonotone = yMonotone;
    }

    public IEnumerator<Point> GetEnumerator() => Points.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Points).GetEnumerator();

    public Point ClosestPoint(Point point)
    {
        var points = Poly8FilterFromPoint(point);
        var exactLenSquared = points.Select(point => (point, new Vector(point.X, point.Y).LengthSquared));
        var comparer = Comparer<(Point point, double lenSquared)>
            .Create((p, q) => p.lenSquared.CompareTo(q.lenSquared));
        var min = exactLenSquared.Min(comparer);

        return min.point;
    }

    private IEnumerable<Point> Poly8FilterFromPoint(Point point)
    {
        var lens = Points.Select(x => point - x).Select(v => v.Poly8Length());
        double min = lens.Min();
        IEnumerable<(Point point, double len)> zip = Points.Zip(lens);
        return zip
            .Where(p => p.len - min <= VectorExtensions.Poly8Tolerance)
            .Select(u => u.point);
    }
}
