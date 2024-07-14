using System.Collections;
using System.Windows;

namespace ParametricCurves;

public class Curve : IEnumerable<Point>
{
    internal Contour? _contour;
    public bool IsClosed { get => _isClosed; set => _isClosed = value; }
    private List<Monotone2D> _monotones;
    private bool _isClosed;

    public List<Monotone2D> Monotone => _monotones;
    public IEnumerable<Point> Points => _monotones.SelectMany(x => x.Points);

    public IEnumerator<Point> GetEnumerator() => Points.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Points).GetEnumerator();
}