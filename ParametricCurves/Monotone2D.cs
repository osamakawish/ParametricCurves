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

    // Check saved Desmos graph for efficient approach. Titled "Efficient Distance Check".
    // Idea: Use octagonal filter.
    public Vector ShortestVectorFromPoint(Point point)
    {
        var points = OctagonalFilterFrom(point);

        throw new NotImplementedException();
    }

    private HashSet<Point> OctagonalFilterFrom(Point point)
    {
        var vecs = Points.Select(x => point - x).Select(v => v.OctagonLength());
        

        throw new NotImplementedException();
    }
}
