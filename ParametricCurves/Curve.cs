using System.Collections;
using System.Windows;

namespace ParametricCurves;

public class Curve : IEnumerable<Point>
{
    internal Contour? _contour;
    public bool IsClosed { get => _isClosed; set => _isClosed = value; }
    private List<Monotone2D> _monotones = [];
    private bool _isClosed;

    // More efficient if stored.
    public Rect Boundary => _monotones.Select(m => m.Range.AsRect).Aggregate(Rect.Union);

    public bool ClockwiseOrientation { get; set; }

    public List<Monotone2D> Monotones => _monotones;
    public IEnumerable<Point> Points => _monotones.SelectMany(x => x.Points);

    public IEnumerator<Point> GetEnumerator() => Points.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Points).GetEnumerator();

    /// <summary>
    /// Applies the desired combination to the current curve, placing both under the same contour.
    /// </summary>
    /// <param name="combination"></param>
    /// <param name="other"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(CurveCombination combination, Curve other) => throw new NotImplementedException();

    // SLOW: Just iteratively intersects every pair of monotones.
    public IEnumerable<Point> PointsOfIntersection(Curve other, double tolerance)
        => _monotones.SelectMany(x => other._monotones.SelectMany(y => x.Intersect(y, tolerance)));

    public Contour Intersect(Curve other) => (IsClosed, other.IsClosed) switch
    {
        (false, false) => Contour.Empty,
        (true, false) => IntersectClosedWithOpenCurve(this, other),
        (false, true) => IntersectClosedWithOpenCurve(other, this),
        _ => IntersectClosedCurves(this, other)
    };

    private Contour IntersectClosedCurves(Curve curve, Curve other) => throw new NotImplementedException();
    private Contour IntersectClosedWithOpenCurve(Curve closed, Curve open) => throw new NotImplementedException();

    public void ApplyIntersect(Curve other) => Apply(Intersect, other);
    public Contour Union(Curve other) => throw new NotImplementedException();
    public void ApplyUnion(Curve other) => Apply(Union, other);
    public Contour Exclude(Curve other) => throw new NotImplementedException();
    public void ApplyExclude(Curve other) => Apply(Exclude, other);
    public Contour Subtract(Curve other) => throw new NotImplementedException();
    public void ApplySubtract(Curve other) => Apply(Subtract, other);
}