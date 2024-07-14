using System.Windows;

namespace ParametricCurves.PointCollection;



/// <summary>
/// Uses a pair of Binary Search Tree to store points.
/// </summary>
public class PointCollection
{
    public IList<Point> PointsSortedByX { get; set; }
    public IList<Point> PointsSortedBYY { get; set; }

    PointNode Parent { get; set; }
    PointNode LesserNode { get; set; }
    PointNode GreaterNode { get; set; }

    bool IsXCoordinate { get; }

    /// <summary>
    /// The other half of the point.
    /// </summary>
    PointCollection Other { get; set; }

    public PointCollection()
    {
        Parent = null;
    }

    public PointCollection(Point point)
    {

    }
}