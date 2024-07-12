using System.Windows;
using System.Windows.Shapes;

namespace ParametricCurves;

public record PointRange(Point Start, Point End)
{
    public static Point NaNPoint { get; } = new(double.NaN, double.NaN);
    public bool IsNaN => Start.IsNaNPoint() || End.IsNaNPoint();
    public static PointRange NaN { get; } = new(NaNPoint, NaNPoint);

    public static implicit operator PointRange((Point point1, Point point2) points)
        => new(points.point1, points.point2);

    public static implicit operator Rect(PointRange pointRange) => pointRange.IsNaN ?
        Rect.Empty : new(pointRange.Start, pointRange.End);

    public static implicit operator PointRange(Line line)
        => new(Start: new(line.X1, line.Y1), End: new(line.X2, line.Y2));

    public Line ToLine() => new() { X1 = Start.X, Y1 = Start.Y, X2 = End.X, Y2 = End.Y };
}