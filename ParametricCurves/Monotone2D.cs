using System.Collections;
using System.Windows;

namespace ParametricCurves;

public class Monotone2D : IEnumerable<Point>
{
    private readonly Monotone _xMonotone;
    private readonly Monotone _yMonotone;
    public IEnumerable<Point> Points => _xMonotone.Zip(_yMonotone).Select(x => new Point(x.First, x.Second));

    public (bool xForward, bool yForward) Orientation => (_xMonotone.Forward, _yMonotone.Forward);

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


}